using ProtobufWebsocket.Assembly_Helpers;
using ProtobufWebsocket.Broadcast_Helper;
using ProtobufWebsocket.Endpoint_Provider;
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
            var requestEndpointType = EndpointsTypeProvider.getRequestInstance();

            var RequestEndpointObject = ProtobufAccessHelper.Decode(requestEndpointType, incomingBytes); //class includes list of objects of extended type irequest

            List<(object request, EndpointTypeProperties endpointProp)> CalledEndpoint = EndpointHelper.EndpointHelper.getAssociatedEndpoint(RequestEndpointObject);

            List<(object requestObject, object EndpointObject)> endpoints = CalledEndpoint.Select(E => {
                return (E.request, EndpointHelper.EndpointHelper.prepareEndpointObject(E.endpointProp));
            }).ToList();

            byte[] encoded = null;

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

                encoded = EndpointHelper.EndpointHelper.handle(endpointWithUID, requestObject);


            }

            return encoded;
        }
    }
}
