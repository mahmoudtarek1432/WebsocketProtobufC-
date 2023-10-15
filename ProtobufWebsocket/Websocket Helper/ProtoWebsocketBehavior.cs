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
            var requestProto = ProtoFileProvider.GetRequestFile();
            var responseProto = ProtoFileProvider.GetResponseFile();

            Send(requestProto);
            Send(responseProto);

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
