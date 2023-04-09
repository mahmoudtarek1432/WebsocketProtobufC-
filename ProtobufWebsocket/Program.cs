// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProtoBuf.Meta;
using ProtobufWebsocket.Assembly_Helpers;
using ProtobufWebsocket.Attributes;
using ProtobufWebsocket.Dependency_Injection;
using ProtobufWebsocket.Endpoint_Provider;
using ProtobufWebsocket.EndpointHelper;
using ProtobufWebsocket.Model;
using ProtobufWebsocket.Protobuf_Helper;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

Console.WriteLine("Hello, World!");
using IHost host = Host.CreateDefaultBuilder(args).ConfigureServices(services =>
{
    services.AddWebsocketDependencyInjection();

}).Build();

var a = Assembly.GetExecutingAssembly();
var module = ProtoAssemblyBuilder.DefineNewModule("endpoint");


var newtype =  EndpointFactory.PrepareForProto((typeof(ProductResponse), "r"), module).CreateType();
var obj = Activator.CreateInstance(newtype);
newtype.GetRuntimeFields().ToList().ForEach(i => 
{
    i.SetValue(obj, null);
    Console.WriteLine(i.Name + " " + i.FieldType.Name + " " + i.FieldType.IsClass);
    Console.WriteLine();

});



var product = new ProductResponse() { Name = "name", Description = "desc", Price = 5 };

var arr = Array.CreateInstance(newtype,5);
var t = arr.GetType().Name.Replace("[]","");

ProtobufCreationHelper.IntializeProtoEnvironment("endpoint", a);

var res = EndpointsTypeProvider.getResponseInstance();
var instance = Activator.CreateInstance(res);
instance.GetType().GetRuntimeFields().ToList().ForEach(f =>
{
    var obj = Activator.CreateInstance(f.FieldType,new object[] {3});
    var x = obj.GetType();/*.GetRuntimeFields().ToList().ForEach(O =>
    {
        var prop = product.GetType().GetProperty(O.Name);
        O.SetValue(obj, prop.GetValue(product));
    });*/
    f.SetValue(instance, obj); 
});

var x = new ProductResponse[] {product, product , product };
var s = x.GetType();
var Arr = Activator.CreateInstance(s,new object[] {1});
var num = RuntimeArrayHelpers.AppendObjectToRuntimeArray(x, product);
var o = RuntimeArrayHelpers.extendRuntimeArray(x);

var endpoint = ProtobufAccessHelper.fillEndpoint(product);
var serialized = ProtobufAccessHelper.Encode(endpoint);


using(MemoryStream ms = new MemoryStream(serialized))
{
    var de = ProtoBuf.Serializer.Deserialize(endpoint.GetType(), ms);
    Console.WriteLine();
}


Console.WriteLine();