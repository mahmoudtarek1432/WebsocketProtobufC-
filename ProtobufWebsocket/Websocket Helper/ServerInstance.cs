using WebSocketSharp.Server;

namespace ProtobufWebsocket.Websocket_Helper
{
    internal class SessionInstance
    {
        static WebSocketSessionManager Session { get; set; }

        public static WebSocketSessionManager GetSessionManagerInstance()
        {
            if(Session == null)
            {
                throw new ArgumentNullException();
            }
            return Session;
        }

        public static void CreateSessionInstance(WebSocketSessionManager session)
        {
            Session = session;
        }
    }
}
