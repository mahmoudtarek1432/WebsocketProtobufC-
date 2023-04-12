using ProtobufWebsocket.Broadcast_Helper;
using ProtobufWebsocket.EndpointHelper;
using ProtobufWebsocket.Websocket_Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.Broadcast_Helper
{
    internal class BroadcastService : IBroadcastService
    {
        public void EndpointBroadCast<EndpointHandler>(EndpointHandler handler) where EndpointHandler : IDynamicEndpoint
        {
            var endpointusers = broadcastDictionaryProvider.GetEndpointUsers(handler.GetType().Name);

            var server = ServerInstance.getServerInstance();

           
        }
    }
}
