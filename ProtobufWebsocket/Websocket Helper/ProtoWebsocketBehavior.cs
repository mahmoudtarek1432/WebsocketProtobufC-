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
            Console.WriteLine($"socket started with id:{ID}");
            base.OnOpen();
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            //var bytes = new byte[] { 10, 16, 10, 4, 100, 97, 109, 101, 18, 4, 100, 101, 115, 99, 26, 2, 8, 5 };
            //Console.WriteLine($"message recieved from user {ID}: {e.Data}");
            var responseObject = ProtoEndpointApi.ResolveRequest(e.RawData, ID);
            Send(responseObject);
            //Send(returnTestBytes());
            base.OnMessage(e);
        }

        public byte[] returnTestBytes()
        {
            var bytes = new byte[] {10,6,32,1,40,0,48,2};
            return bytes;
        }
    }
}
