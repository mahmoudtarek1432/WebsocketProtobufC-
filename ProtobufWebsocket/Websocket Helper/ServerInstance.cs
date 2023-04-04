using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace ProtobufWebsocket.Websocket_Helper
{
    internal class ServerInstance
    {
        static WebSocketServer _server { get; set; }

        public static WebSocketServer getServerInstance(string address = null)
        {
            if(_server == null)
            {
                _server = (address == null)? new WebSocketServer() : new WebSocketServer(address);

                return _server;
            }
            return _server;
        }
    }
}
