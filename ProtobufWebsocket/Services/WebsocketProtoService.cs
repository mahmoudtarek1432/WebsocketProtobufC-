using Microsoft.Extensions.DependencyInjection;
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
using ProtobufWebsocket.Broadcast_Helper;

namespace ProtobufWebsocket.Services
{
    public static class WebsocketProtoService
    {
        /**
         * <summary> Adds ProtoWesocketService to the Host services specified in
         * <see cref="IServiceCollection"/> and starts up the connection.
         * Notice: add DI services prior to adding this service.
           </summary>
           <typeparam name="address">The type of the service to add.</typeparam>
           <typeparam name="TImplementation">The type of the implementation to use.</typeparam>
        <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        <param name="address">The Websocket address clients shall connect to. For example: ws://127.0.0.1:80</param>
        <param name="path">The path service will connect do, the default path is "/proto".</param>
        <param name="assembly">The excuting in code assemblies. </param>
        
           <returns>A reference to this instance after the operation has completed.</returns>
         */
        public static IServiceCollection AddProtoWebsocketService(this IServiceCollection services, System.Reflection.Assembly assembly,string address, string path="/proto")
        {
            services.AddSingleton<IBroadcastService, BroadcastService>(); 

            var ServiceProvider = services.BuildServiceProvider();
            UseProtoWebsocketServiceDI(ServiceProvider);
            //fetch for types accross the running code with attribute names given
            ProtobufCreationHelper.IntializeProtoEnvironment("endpoint", assembly);

            //prepares endpoints and appends a singleton containing all the needed properties
            EndpointHelper.EndpointHelper.PrepareEndpointHandlers(assembly);

            //start protoservice
            var server = WebsocketBuilder.ConfigureServer(address,path);
           
            server.Start();

            return services;
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
