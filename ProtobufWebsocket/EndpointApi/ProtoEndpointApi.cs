using ProtobufWebsocket.Assembly_Helpers;
using ProtobufWebsocket.Broadcast_Helper;
using ProtobufWebsocket.EndpointHelper;
using ProtobufWebsocket.Protobuf_Helper;
using ProtobufWebsocket.RequestMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.EndpointApi
{
    internal class ProtoEndpointApi
    {
        public static byte[] ResolveRequest(byte[] incomingBytes, string userId)//service provider instance that is passed from the application builder context
        {
            List<(object request, EndpointTypeProperties endpointProp)> CalledEndpoint = EndpointHelper.EndpointHelper.getAssociatedEndpoints(incomingBytes);

            List<(object requestObject, object EndpointObject)> endpoints = CalledEndpoint.Select(E =>
            {
                return (E.request, EndpointHelper.EndpointHelper.prepareEndpointObject(E.endpointProp));
            }).ToList();
            var encoded = new byte[] { };
            foreach (var resolve in endpoints)
            {
                if (EndpointHelper.EndpointHelper.CheckIfBroadcast(resolve.requestObject))
                {
                    var endpointType = resolve.EndpointObject.GetType();
                    broadcastDictionaryProvider.AddUserToEndpoint(endpointType.Name, userId);
                }
                //ask about it 

                var endpointWithUID = EndpointHelper.EndpointHelper.PassUserId(resolve.EndpointObject, userId);
                var requestObject = resolve.requestObject;

                var handlerReturnObject = EndpointHelper.EndpointHelper.InvokeHandler(requestObject, endpointWithUID); //returns a task<object>
                var invokeReturnType = AssemblyHelper.resolveTask(handlerReturnObject);
                //serialize and return to user

                var responseEndpoint = ProtobufAccessHelper.fillEndpoint(invokeReturnType, null); //second param to create a new endpoint

                encoded = ProtobufAccessHelper.Encode(responseEndpoint);


            }

            return encoded;
        }
    }
}
