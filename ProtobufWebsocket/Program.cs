// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProtoBuf.Meta;
using ProtobufWebsocket.Assembly_Helpers;
using ProtobufWebsocket.Attributes;
using ProtobufWebsocket.Dependency_Injection;
using ProtobufWebsocket.Endpoint_Provider;
using ProtobufWebsocket.EndpointApi;
using ProtobufWebsocket.EndpointHelper;
using ProtobufWebsocket.Model;
using ProtobufWebsocket.Protobuf_Helper;
using ProtobufWebsocket.Services;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

var a = Assembly.GetExecutingAssembly();


var product = new ProductResponse() { Name = "name", Description = "desc", Price = 5 };

ProtobufCreationHelper.IntializeProtoEnvironment("endpoint", a);
EndpointHelper.PullEndpointThroughAssembly(a);

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
    var z = RuntimeTypeModel.Default.GetSchema(de.GetType());
    Console.WriteLine();
}


var end = typeof(ProtoEndpoint.Request<product>.WithResponse<ProductResponse>);

var d = typeof(product).GetConstructors().First().GetParameters()[1].ParameterType;

EndpointHelper.ResolveRequest(serialized);

Console.WriteLine();

class testendpoint : ProtoEndpoint.Request<product>.WithResponse<ProductResponse>
{
    public override Task<ProductResponse> Handle(product Request)
    {
        Console.WriteLine("ITS WORKING !!!!");
        return null;
    }
}