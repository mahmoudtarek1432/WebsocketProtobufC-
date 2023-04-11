using Microsoft.Extensions.DependencyInjection;
using ProtobufWebsocket.Assembly_Helpers;
using ProtobufWebsocket.Attributes;
using ProtobufWebsocket.Broadcast_Helper;
using ProtobufWebsocket.Dependency_Injection;
using ProtobufWebsocket.Endpoint_Provider;
using ProtobufWebsocket.Model;
using ProtobufWebsocket.Protobuf_Helper;
using ProtobufWebsocket.RequestMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.EndpointHelper
{
    internal class EndpointHelper
    {
        public static void PrepareEndpointHandlers(Assembly assembly)
        {
            var globalTypes = AssemblyHelper.loadAssemblyTypes(assembly);

                //retrieves all the assemblies consuming these attributes
            var Handlers = globalTypes.Where(t =>
                t.GetInterfaces().ToList()
                .Where( I => I.Name == typeof(IDynamicEndpoint).Name).Any());

           foreach ( var handler in Handlers )
            {
                var Requesttype = handler.BaseType!.GetGenericArguments().Where(A => A.BaseType.Name == typeof(IRequest).Name).FirstOrDefault();
                if (Requesttype != null)
                {
                    RequestMappingHelper.MapRequestToEndpoint(Requesttype!, handler);      //maps an endpoint to a request
                    broadcastDictionaryProvider.CreateNewDictionaryInstance(handler.Name); //intialize an endpoint dictionary
                }
            }

            EndpointsHandleProvider.CreateEndpointHandlerSingleton(Handlers.ToList()); //endpoints saved
        }

        public static byte[] ResolveRequest(byte[] incomingBytes, string userId)//service provider instance that is passed from the application builder context
        {
            List<(object request, EndpointTypeProperties endpointProp)> CalledEndpoint = getAssociatedEndpoints(incomingBytes);

            List<(object requestObject, object EndpointObject)> endpoints = CalledEndpoint.Select( E => {
                                                                                            return (E.request, prepareEndpointObject(E.endpointProp));
                                                                                        }).ToList();
            var encoded = new byte[] { };
            foreach (var resolve in endpoints)
            {
                if (CheckIfBroadcast(resolve.requestObject))
                {
                    var endpointType = resolve.EndpointObject.GetType();
                    broadcastDictionaryProvider.AddUserToEndpoint(endpointType.Name, userId);
                }
                //ask about it 

                var endpointWithUID =  PassUserId(resolve.EndpointObject, userId);
                var requestObject = resolve.requestObject;

                var handlerReturnObject = InvokeHandler(requestObject, endpointWithUID); //returns a task<object>
                var invokeReturnType = AssemblyHelper.resolveTask(handlerReturnObject);
                //serialize and return to user

                var responseEndpoint = ProtobufAccessHelper.fillEndpoint(invokeReturnType, null); //second param to create a new endpoint

                encoded = ProtobufAccessHelper.Encode(responseEndpoint);

                
            }

            return encoded;

        }

        //gets called each time a message is recieved, invokes the appropriate endpoint 
        internal static List<(object request, EndpointTypeProperties endpointProp)> getAssociatedEndpoints(byte[] incomingBytes)
        {
           
            var requestEndpointType = EndpointsTypeProvider.getRequestInstance();

            var RequestEndpointObject = ProtobufAccessHelper.Decode(requestEndpointType, incomingBytes); //class includes list of objects of extended type irequest

            var EndpointList = new List<(object request, EndpointTypeProperties endpointProp)>(); //list of tuple that takes a request and endpoint

            requestEndpointType.GetRuntimeFields().ToList().ForEach(field =>
            {
                if (field.FieldType.IsArray)
                {
                    var fieldArr = field.GetValue(RequestEndpointObject);
                    if (fieldArr != null)
                    {
                        
                        RuntimeArrayHelpers.loopRuntimeArray(fieldArr!, (element) =>
                        {
                            //get the Request's coresponding endpoint
                            EndpointList.Add((element, RequestMappingHelper.GetEndpoint(element.GetType())));

                        });
                    }
                }
            });

            return EndpointList;
        }

        //intializes an endpoint along with its constructor parameters
        internal static object prepareEndpointObject(EndpointTypeProperties endpoint) //endpopint is created, dependencies resolved.
        {

            //an array of objects is constructed using dependency injection
            var constructorObjects = new List<object>();

            foreach(var param in endpoint.EndpointConstructorParams)
            {
                var IntializedParam = DependencyInjectionHelper.IntializeWithDI(param);
                constructorObjects.Add(IntializedParam);
            }

            var endpointType = endpoint.EndpointType;

            object endpointInstance = Activator.CreateInstance(endpointType, constructorObjects.ToArray())!; //might have issues with object placment

            return endpointInstance;

        }
        
        internal static object PopulateType(Type StaticType, object runtimeObject) //clones runtime value, the runtime type has the exact same properties and fields
        {
            //getUninitializedObject returns an object of type, without getting instantiated.
            //this is used as the types passed are usually modles and dtos that does not have constructor implementations.
            
            var staticTypeInstance = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(StaticType);

            foreach (var field in staticTypeInstance.GetType().GetProperties())
            {
                var runtimeFieldValue = runtimeObject.GetType().GetField(field.Name).GetValue(runtimeObject);
                field.SetValue(staticTypeInstance, runtimeFieldValue);
            }
            return staticTypeInstance;
        }

        internal static object InvokeHandler(object requestObject, object EndpointObject)
        {
            var endpointHandlerType = EndpointObject.GetType();
            var handleDelegate = endpointHandlerType.GetMethod("Handle");

            var Requesttype = endpointHandlerType.BaseType!.GetGenericArguments().Where(A => A.BaseType.Name == typeof(IRequest).Name).FirstOrDefault();
            var HandlerRequest = PopulateType(Requesttype,requestObject); //returns an instance of the concrete class created as a request type

            return handleDelegate.Invoke(EndpointObject, new object[] { HandlerRequest }); //second argument is the request object
        }

        //checks if the passed object that inherets IRequest is a broadcast subscription request
        internal static bool CheckIfBroadcast(object Request)
        {
            var cr = new checkRequest();

            var Is_subscribeField = Request.GetType().GetRuntimeField(nameof(cr.is_subscribe));

            return (bool) Is_subscribeField!.GetValue(Request)!;

        }

        public static object PassUserId(object endpoint, string UserId)
        {
            var endpointType = endpoint.GetType();
            var field = endpointType.GetField("userGUID");

            if (field == null)
                throw new Exception($"field userGUID is not present in {endpointType.FullName}");

            field.SetValue(endpoint, UserId);

            return endpoint;
        }
    }
}
