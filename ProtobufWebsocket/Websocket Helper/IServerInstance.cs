using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace ProtobufWebsocket.Websocket_Helper
{
    internal interface IServerInstance
    {
        public WebSocketServer server { get; set; }
    }
}
