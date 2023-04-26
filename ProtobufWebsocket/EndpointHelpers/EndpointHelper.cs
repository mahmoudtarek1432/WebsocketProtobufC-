using Microsoft.Extensions.DependencyInjection;
using ProtobufWebsocket.Assembly_Helpers;
using ProtobufWebsocket.Attributes;
using ProtobufWebsocket.Broadcast_Helper;
using ProtobufWebsocket.Dependency_Injection;
using ProtobufWebsocket.Endpoint_Provider;
using ProtobufWebsocket.Extentions;
using ProtobufWebsocket.Model;
using ProtobufWebsocket.Protobuf_Helper;
using ProtobufWebsocket.RequestMapping;
using System.Reflection;
using static ProtobufWebsocket.EndpointApi.ProtoEndpointBase;

namespace ProtobufWebsocket.EndpointHelper
{
    internal class EndpointHelper
    {
        //prepares endpoints by pairing endpoints with thier request types, this is conducted to decrease the processing at runtime,
        //a dynamic programming approach is taken a achive so
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
                    BroadcastDictionaryProvider.CreateNewDictionaryInstance(handler.Name); //intialize an endpoint dictionary
                }
            }

            EndpointsHandleProvider.CreateEndpointHandlerSingleton(Handlers.ToList()); //endpoints saved
        }

        //gets called each time a message is recieved, invokes the appropriate endpoint 
        internal static List<(object request, EndpointTypeProperties endpointProp)> GetAssociatedEndpoint(object RequestObject)
        {
            var EndpointList = new List<(object request, EndpointTypeProperties endpointProp)>(); //list of tuple that takes a request and endpoint

            RequestObject.GetType().GetRuntimeFields().ToList().ForEach(field =>
            {
                if (field.FieldType.IsArray)
                {
                    var fieldArr = field.GetValue(RequestObject);
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
        internal static object PrepareEndpointObject(EndpointTypeProperties endpoint) //endpopint is created, dependencies resolved.
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

        //checks if the passed object that inherets IRequest is a broadcast subscription request
        internal static bool CheckIfBroadcast(object Request)
        {

            var Is_subscribeField = Request.GetType().GetRuntimeField("is_subscribe");

            return (bool) Is_subscribeField!.GetValue(Request)!;

        }

        public static object PassUserId(object endpoint, string UserId)
        {
            var endpointType = endpoint.GetType();
            var field = endpointType.GetField("UserId");

            if (field == null)
                throw new Exception($"field userGUID is not present in {endpointType.FullName}");

            field.SetValue(endpoint, UserId);

            return endpoint;
        }

        public static object PassResponseTheRequestId(object Request, object Response)
        {
            var requestId = Request.GetType().GetRuntimeField("request_id").GetValue(Request);

            Response.GetType().GetProperty("request_id").SetValue(Response, requestId);

            return Response;
        }

        public static object Handle(object EndpointObject, object requestObject)
        {
             var handlerReturnObject = EndpointObject.InvokeHandler(requestObject); //returns a task<object>
            
            var invokeReturnType = AssemblyHelper.resolveTask(handlerReturnObject);
            return invokeReturnType;
        }

        public static (Type, string) identifyEndpoint(Type type)
        {

            var Reqatt = type.GetCustomAttributes().Where(A =>
                A.GetType().Name == typeof(EndpointRequestAttribute).Name);

            if (Reqatt.Any())
            {
                return (type, "request");
            }
            return (type, "response");
        }
    }
}
