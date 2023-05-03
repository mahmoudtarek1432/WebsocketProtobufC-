using WebSocketSharp;
using ProtobufWebsocket.EndpointApi;
using WebSocketSharp.Server;
using ProtobufWebsocket.ProtoFileConstructor;

namespace ProtobufWebsocket.Websocket_Helper
{
    internal class ProtoWebsocketBehavior : WebSocketBehavior
    {
        protected override void OnOpen()
        {
            Console.WriteLine($"socket started with id:{ID}");
            //send request and response proto files
            var requestProto = ProtoFileProvider.getRequestFile();
            var responseProto = ProtoFileProvider.getResponseFile();

            Send(requestProto);
            Send(responseProto);

            base.OnOpen();
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            var bytes = new byte[] { 10,46,10,7,109,97,104,109,111,117,100,18,24,68,101,115,99,114,105,112,116,105,111,110,58,32,102,114,111,109,32,99,108,105,101,110,116,29,0,0,160,65,32,1,40,1,48,2};
            //Console.WriteLine($"message recieved from user {ID}: {e.Data}");
            var responseObject = ProtoEndpointApi.ResolveRequest(bytes, ID);
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
