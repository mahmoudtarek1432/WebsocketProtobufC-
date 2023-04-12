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
        static WebSocketSessionManager _Session { get; set; }

        public static WebSocketSessionManager getSessionInstance()
        {
            if(_Session == null)
            {
                throw new ArgumentNullException();
            }
            return _Session;
        }

        public static void createSessionInstance(WebSocketSessionManager session)
        {
            _Session = session;
        }
    }
}
