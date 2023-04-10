using Microsoft.Extensions.DependencyInjection;
using ProtobufWebsocket.Assembly_Helpers;
using ProtobufWebsocket.Attributes;
using ProtobufWebsocket.Dependency_Injection;
using ProtobufWebsocket.Endpoint_Provider;
using ProtobufWebsocket.Model;
using ProtobufWebsocket.Protobuf_Helper;
using ProtobufWebsocket.RequestMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.EndpointHelper
{
    internal class EndpointHelper
    {
        public static void PullEndpointThroughAssembly(Assembly assembly)
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
                    RequestMappingHelper.MapRequestToEndpoint(Requesttype!, handler);
                }
            }

            EndpointsHandleProvider.CreateEndpointHandlerSingleton(Handlers.ToList()); //endpoints saved
        }

        public static void ResolveRequest(byte[] incomingBytes)//service provider instance that is passed from the application builder context
        {
            List<(object request, EndpointTypeProperties endpointProp)> CalledEndpoint = getAssociatedEndpoints(incomingBytes);

            List<(object requestObject, object EndpointObject)> endpoint = CalledEndpoint.Select( E => {
                return (E.request, prepareEndpoint(E.endpointProp));
            }).ToList();

            foreach (var resolve in endpoint)
            {
                var endpointHandlerType = resolve.EndpointObject.GetType();
                var handleDelegate = endpointHandlerType.GetMethod("Handle");

                var Requesttype = endpointHandlerType.BaseType!.GetGenericArguments().Where(A => A.BaseType.Name == typeof(IRequest).Name).FirstOrDefault();
                var HandlerRequest = PopulateType(Requesttype, resolve.requestObject); //returns an instance of the concrete class created as a request type

                handleDelegate.Invoke(resolve.EndpointObject, new object[] { HandlerRequest }); //second argument is the request object
            }



        }

        //gets called each time a message is recieved, invokes the appropriate endpoint 
        private static List<(object request, EndpointTypeProperties endpointProp)> getAssociatedEndpoints(byte[] incomingBytes)
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
        private static object prepareEndpoint(EndpointTypeProperties endpoint) //endpopint is created, dependencies resolved.
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

        public void invokeEndpointHandle(object endpoint)
        {

        }
        

        public static object PopulateType(Type StaticType, object runtimeObject) //clones runtime value, the runtime type has the exact same properties and fields
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
    }
}
