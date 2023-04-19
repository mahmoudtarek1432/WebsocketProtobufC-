using WebSocketSharp.Server;

namespace ProtobufWebsocket.Websocket_Helper
{
    internal class SessionInstance
    {
        static WebSocketSessionManager _Session { get; set; }

        public static WebSocketSessionManager getSessionManagerInstance()
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
