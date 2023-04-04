using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace ProtobufWebsocket.Websocket_Helper
{
    internal class ProtoWebsocketBehavior : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            
            base.OnOpen();
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            base.OnMessage(e);
        }
    }
}
