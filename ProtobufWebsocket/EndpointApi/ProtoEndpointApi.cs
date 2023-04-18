using ProtobufWebsocket.Assembly_Helpers;
using ProtobufWebsocket.Broadcast_Helper;
using ProtobufWebsocket.Endpoint_Provider;
using ProtobufWebsocket.EndpointHelper;
using ProtobufWebsocket.Model;
using ProtobufWebsocket.Protobuf_Helper;
using ProtobufWebsocket.RequestMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.EndpointApi
{
    internal class ProtoEndpointApi
    {
        public static byte[] ResolveRequest(byte[] incomingBytes, string userId)//service provider instance that is passed from the application builder context
        {
            var requestEndpointType = EndpointsTypeProvider.getRequestInstance();

            var RequestEndpointObject = ProtobufAccessHelper.Decode(requestEndpointType, incomingBytes); //class includes list of objects of extended type irequest

            List<(object request, EndpointTypeProperties endpointProp)> CalledEndpoint = EndpointHelper.EndpointHelper.GetAssociatedEndpoint(RequestEndpointObject);

            List<(object requestObject, object EndpointObject)> endpoints = CalledEndpoint.Select(E => {
                return (E.request, EndpointHelper.EndpointHelper.PrepareEndpointObject(E.endpointProp));
            }).ToList();

            byte[] encoded = null;

            foreach (var resolve in endpoints)
            {
                //ask about it 

                var endpointWithUID = EndpointHelper.EndpointHelper.PassUserId(resolve.EndpointObject, userId);
                var requestObject = resolve.requestObject;


                var ResponseObject = EndpointHelper.EndpointHelper.Handle(endpointWithUID, requestObject);

                EndpointHelper.EndpointHelper.PassResponseTheRequestId(requestObject, ResponseObject);

                if (EndpointHelper.EndpointHelper.CheckIfBroadcast(resolve.requestObject))
                {
                    var endpointType = resolve.EndpointObject.GetType();
                    if(!BroadcastDictionaryProvider.AddUserToEndpoint(endpointType.Name, userId))
                    {
                        ResponseObject = addRuntimeError(ResponseObject, new Error() { message = "The client is subscribed"});
                    }

                }
                var responseEndpoint = ProtobufAccessHelper.fillEndpoint(ResponseObject, null); //second param to create a new endpoint

                encoded=  ProtobufAccessHelper.Encode(responseEndpoint);
            }

            return encoded;
        }

        public static object addRuntimeError(object ResponseObject,Error error)
        {
            var errorsProperty = ResponseObject.GetType().GetProperty("Errors");
            var errorArrayValue = errorsProperty?.GetValue(ResponseObject);
            if(errorArrayValue == null)
            {
                var type = ResponseObject.GetType().GetProperty("Errors")!.PropertyType.GetGenericArguments().FirstOrDefault();
                errorArrayValue = Array.CreateInstance(type, 1);
            }
            errorArrayValue = AppendStaticArray(errorArrayValue, error);
            errorsProperty.SetValue(ResponseObject, errorArrayValue);
            return ResponseObject;
        }

        public static object AppendStaticArray(object array, object ToGetAppended)
        {
            var arrayType = array.GetType();
            if (!array.GetType().IsArray)
                throw new Exception($"The passed is object is not an instance of Type Array {nameof(AppendStaticArray)}");
            var name = arrayType.GetElementType()!.Name;
            if (!(arrayType.GetElementType()!.Name == ToGetAppended.GetType().Name)) //element type and object are not the same
                throw new Exception($"array and object are not of the same type (Reflection), thrown at {nameof(AppendStaticArray)}");

            var elementType = arrayType.GetElementType();                                                                               //used to Traverse inside the object
            var elementObject = Activator.CreateInstance(arrayType.GetElementType()!);

            var emptyelementType = elementObject.GetType();
            var test = emptyelementType.GetProperties();
            emptyelementType.GetProperties().ToList().ForEach(e =>
            {
                e.SetValue(elementObject, ToGetAppended.GetType().GetProperty(e.Name).GetValue(ToGetAppended)); //searches the to be cloned object for field name and sets the element value
            });

            //arrayType
            var AppendedArray = RuntimeArrayHelpers.AppendObjectToRuntimeArray(array, elementObject);

            return AppendedArray;


        }
    }
}
