// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Hosting;
using ProtobufWebsocket.Services;
using System.Reflection;

Console.WriteLine("Hello, World!");

Host.CreateDefaultBuilder().ConfigureServices(s =>
{
    s.AddProtoWebsocketService("", "d", Assembly.GetExecutingAssembly());
}).Build();
