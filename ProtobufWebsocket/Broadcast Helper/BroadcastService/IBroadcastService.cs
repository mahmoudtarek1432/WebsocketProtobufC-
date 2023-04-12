using ProtobufWebsocket.EndpointHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.Broadcast_Helper
{
    public interface IBroadcastService
    {
        /**
         * Sends all the subscribed users a response, triggered by the server
         */
        public void EndpointBroadCast<EndpointHandler>(EndpointHandler handler) where EndpointHandler : IDynamicEndpoint;
    }
}
