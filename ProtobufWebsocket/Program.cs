﻿// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProtoBuf.Meta;
using ProtobufWebsocket.Assembly_Helpers;
using ProtobufWebsocket.Attributes;
using ProtobufWebsocket.Dependency_Injection;
using ProtobufWebsocket.Endpoint_Provider;
using ProtobufWebsocket.EndpointApi;
using ProtobufWebsocket.EndpointHelper;
using ProtobufWebsocket.Protobuf_Helper;
using ProtobufWebsocket.Services;
using ProtobufWebsocket.TestModels;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;


var Reqlist = new List<product>();
Reqlist.Add(new product { Name = "hello" });

var Reslist = new List<ProductResponse>();
var Reslistclone = new List<ProductResponseClone>();
Reslist.Add(new ProductResponse { Description = "hello" });
Reslistclone.Add(new ProductResponseClone { Description = "hello from clone" });

var x = new RequestEndpoint { product = Reqlist };
var z = new ResponseEndpoint { ProductResponse = Reslist, ProductResponseClone = Reslistclone };

var c = ProtoBuf.Meta.RuntimeTypeModel.Default.GetSchema(typeof(ResponseEndpoint));

var v = ProtobufAccessHelper.Encode(z);
var d = ProtobufAccessHelper.Decode(x.GetType(), v);
Console.WriteLine();























/*var a = Assembly.GetExecutingAssembly();


var product = new ProductResponse() { Name = "name", Description = "desc", Price = 5 };

ProtobufCreationHelper.IntializeProtoEnvironment("endpoint", a);
EndpointHelper.PrepareEndpointHandlers(a);

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


var end = typeof(ProtoEndpointBase.Request<product>.WithResponse<ProductResponse>);

//var d = typeof(product).GetConstructors().First().GetParameters()[1].ParameterType;

EndpointHelper.ResolveRequest(serialized,"1");

Console.WriteLine();

class testendpoint : ProtoEndpointBase.Request<product>.WithResponse<ProductResponse>
{
    public override async Task<ProductResponse> Handle(product Request)
    {
        Console.WriteLine("ITS ALIVE !!!!");
        return new ProductResponse() { Name = "name", Description = "desc", Price = 5 };
    }
}*/