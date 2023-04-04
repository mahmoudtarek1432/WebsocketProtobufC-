// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProtobufWebsocket.Dependency_Injection;
using System.Reflection;

Console.WriteLine("Hello, World!");
using IHost host = Host.CreateDefaultBuilder(args).ConfigureServices(services =>
{
    services.AddWebsocketDependencyInjection();

}).Build();

var a = Assembly.GetExecutingAssembly();
var list = a.GetTypes().ToList();
var classes = list.Where(t => t.IsClass == true).ToList();
classes.ForEach(t =>
{
    t.GetCustomAttributes().ToList().ForEach(I =>
    {
        Console.WriteLine(t.Name + " " + I.GetType().Name);
    });
});