using ProtobufWebsocket.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace ProtobufWebsocket.Websocket_Helper
{
    /** 
     class is responsible for instantiating and accepting websocket requests
     */ 
    public static class WebsocketBuilder
    {
        // use ws for insecure and wss for secure connection
        public static WebSocketServer ConfigureServer(string Address, string Path)
        {
            if (string.IsNullOrEmpty(Address))
                throw new ArgumentNullException($"the passed address is empty or null {nameof(ConfigureServer)}");

            var server = new WebSocketServer(Address);
            server.AddWebSocketService<ProtoWebsocketBehavior>(Path);
            server.KeepClean = false; //doesnt kill in active sessions

            if (server.WebSocketServices.TryGetServiceHost(Path,out var host))
            {
                SessionInstance.createSessionInstance(host.Sessions);
            }
           

            return server;
        }
    }
}
