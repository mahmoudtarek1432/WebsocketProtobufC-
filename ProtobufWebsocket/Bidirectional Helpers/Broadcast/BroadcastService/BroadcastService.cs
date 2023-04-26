using ProtobufWebsocket.Broadcast_Helper;
using ProtobufWebsocket.Dependency_Injection;
using ProtobufWebsocket.EndpointHelper;
using ProtobufWebsocket.Model;
using ProtobufWebsocket.Protobuf_Helper;
using ProtobufWebsocket.RequestMapping;
using ProtobufWebsocket.Websocket_Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProtobufWebsocket.Broadcast_Helper
{
    internal class BroadcastService : IBroadcastService
    {
        //class gets triggered from user assembly so no need for reflection
        public void EndpointBroadCast<Request>(Request request) where Request : IRequest
        {

            var endpointProperties = RequestMappingHelper.GetEndpoint(request.GetType());

            var endpointusers = BroadcastDictionaryProvider.GetEndpointUsers(endpointProperties.EndpointType.GetType().Name);

            var endpointConstructorParams = endpointProperties.EndpointConstructorParams.Select(DependencyInjectionHelper.IntializeWithDI);

            var endpointObject = Activator.CreateInstance(endpointProperties.EndpointType, endpointConstructorParams);

            //endpoint is not uniquely identified, user id will be 0
            endpointObject = EndpointHelper.EndpointHelper. PassUserId(endpointObject, "broadcast");

            var handleReturn = EndpointHelper.EndpointHelper.Handle(endpointObject, request);

            var responseEndpoint = ProtobufAccessHelper.fillEndpoint(handleReturn, null); //second param to create a new endpoint

            var sentbytes =  ProtobufAccessHelper.Encode(responseEndpoint);

            var sessions = SessionInstance.getSessionManagerInstance();

            //invoke handler

            Task.Run(delegate { 
                    //potential problem with write requests
                    foreach (var userId in endpointusers)
                {
                    sessions[userId].Context.WebSocket.SendAsync(sentbytes, (b) => Console.Write("BroadCastSent"));
                }
            });
        }
    }
}
