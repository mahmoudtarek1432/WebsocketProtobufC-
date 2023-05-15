// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProtobufWebsocket.Bidirectional_Helpers.Notification.Notification_Service;
using ProtobufWebsocket.EndpointApi;
using ProtobufWebsocket.Model;
using ProtobufWebsocket.Services;
using ProtobufWebsocket.TestServices;
using System.Reflection;


var host = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.AddProtoWebsocketService(Assembly.GetExecutingAssembly(), "ws://127.0.0.1/", "/test");
});

host.Build().Run();
