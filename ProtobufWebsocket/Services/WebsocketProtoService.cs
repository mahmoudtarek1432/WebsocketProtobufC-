using Microsoft.Extensions.DependencyInjection;
using ProtobufWebsocket.Assembly_Helpers;
using ProtobufWebsocket.Extentions;
using ProtobufWebsocket.Protobuf_Helper;
using ProtobufWebsocket.Websocket_Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.Services
{
    public static class WebsocketProtoService
    {
        public static void AddProtoWebsocketService(this IServiceProvider Service, string address, System.Reflection.Assembly assembly)
        {
            var Types = assembly.getAssemblyWithAttributeName("");

            var server = WebsocketBuilder.ConfigureServer(address);
            server.Start();
        }

    }
}
