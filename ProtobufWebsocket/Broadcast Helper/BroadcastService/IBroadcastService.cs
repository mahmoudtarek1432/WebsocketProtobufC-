using ProtobufWebsocket.EndpointHelper;
using ProtobufWebsocket.Model;
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
        public void EndpointBroadCast<Request>(Request request) where Request : IRequest;
    }
}
