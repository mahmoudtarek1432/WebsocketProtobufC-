// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProtoBuf.Meta;
using ProtobufWebsocket.Assembly_Helpers;
using ProtobufWebsocket.Attributes;
using ProtobufWebsocket.Dependency_Injection;
using ProtobufWebsocket.EndpointHelper;
using ProtobufWebsocket.Model;
using ProtobufWebsocket.Protobuf_Helper;
using System.Reflection;

Console.WriteLine("Hello, World!");
using IHost host = Host.CreateDefaultBuilder(args).ConfigureServices(services =>
{
    services.AddWebsocketDependencyInjection();

}).Build();

var a = Assembly.GetExecutingAssembly();
ProtobufHelper.IntializeProtoEnvironment("endpoint",a);

var module = ProtoAssemblyBuilder.DefineNewModule("endpoint");


var newtype =  EndpointFactory.ConvertIntoAnEndpoint(typeof(product), module).CreateType();
var obj = Activator.CreateInstance(newtype);
/*newtype.GetRuntimeProperties().ToList().ForEach(i => 
{
    i.SetValue(obj, null);
    Console.WriteLine(i.Name + "" + i.GetValue(obj));
});
newtype.GetRuntimeProperty("Name");
*/

var file = RuntimeTypeModel.Default.GetSchema(newtype, ProtoSyntax.Default);
Console.WriteLine();

