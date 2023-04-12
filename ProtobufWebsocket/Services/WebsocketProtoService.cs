﻿using Microsoft.Extensions.DependencyInjection;
using ProtobufWebsocket.Assembly_Helpers;
using ProtobufWebsocket.Dependency_Injection;
using ProtobufWebsocket.EndpointHelper;
using ProtobufWebsocket.Extentions;
using ProtobufWebsocket.Protobuf_Helper;
using ProtobufWebsocket.Websocket_Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.Services
{
    public static class WebsocketProtoService
    {
        public static void AddProtoWebsocketService(this IServiceCollection Service, string address, System.Reflection.Assembly assembly)
        {
            UseProtoWebsocketServiceDI(Service.BuildServiceProvider());
            //fetch for types accross the running code with attribute names given
            ProtobufCreationHelper.IntializeProtoEnvironment("endpoint", assembly);

            //prepares endpoints and appends a singleton containing all the needed properties
            EndpointHelper.EndpointHelper.PrepareEndpointHandlers(assembly);

            //start protoservice
            var server = WebsocketBuilder.ConfigureServer(address);
           
            server.Start();
        }

        public static void UseProtoWebsocketServiceDI(this IServiceProvider serviceProvider)
        {
            ExcutingServiceProvider.CreateInstance(serviceProvider);
        }

        public static void UseProtoWebsocketServiceDI(this IApplicationBuilder applicationBuilder)
        {
            var ServiceProvider = applicationBuilder.ApplicationServices;
            ExcutingServiceProvider.CreateInstance(ServiceProvider);
        }
    }
}
