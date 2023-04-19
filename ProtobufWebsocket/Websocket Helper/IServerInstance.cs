using WebSocketSharp.Server;

namespace ProtobufWebsocket.Websocket_Helper
{
    internal interface IServerInstance
    {
        public WebSocketServer server { get; set; }
    }
}
