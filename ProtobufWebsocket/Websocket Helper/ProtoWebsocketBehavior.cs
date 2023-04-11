using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using ProtobufWebsocket.EndpointApi;
using WebSocketSharp.Server;
using ProtobufWebsocket.Assembly_Helpers;
using ProtobufWebsocket.Model;
using ProtobufWebsocket.Protobuf_Helper;

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
            var responseObject = ProtoEndpointApi.ResolveRequest(e.RawData, ID);
            Send(responseObject);
            base.OnMessage(e);
        }
    }
}
