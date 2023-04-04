﻿using System;
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
    [HelpKeyword]
    public static class WebsocketBuilder
    {
        // use ws for insecure and wss for secure connection
        public static WebSocketServer ConfigureServer(string Address)
        {
            if (string.IsNullOrEmpty(Address))
                throw new ArgumentNullException("address");

            var server = ServerInstance.getServerInstance(Address);
            server.AddWebSocketService<ProtoWebsocketBehavior>("");
            return server;
        }
    }
}