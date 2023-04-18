// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProtobufWebsocket.Services;
using System.Reflection;
using TestPackage.Services;

Console.WriteLine("Hello, World!");

var builder = Host.CreateDefaultBuilder();

builder.ConfigureServices(s =>
{
    s.AddSingleton<IWeatherService, WeatherService>();
    s.AddProtoWebsocketService(Assembly.GetExecutingAssembly(), "ws://127.0.0.1/", "/test");
});

IEnumerable<int> s = null;
s.ToArray();