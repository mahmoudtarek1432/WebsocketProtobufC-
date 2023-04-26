// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProtobufWebsocket.Bidirectional_Helpers.Notification.Notification_Service;
using ProtobufWebsocket.EndpointApi;
using ProtobufWebsocket.Model;
using ProtobufWebsocket.Services;
using ProtobufWebsocket.TestServices;
using System.Reflection;

Host.CreateDefaultBuilder(args).ConfigureServices(s =>
{
    s.AddTransient<INameingService, NamingService>();
    s.AddProtoWebsocketService(Assembly.GetExecutingAssembly(),"ws://127.0.0.1/","/test");
}).Build();

new NotificationService().SendNotification<TestNotifications>();

Console.ReadLine();

//WebsocketProtoService.AddProtoWebsocketService()

/*
var a = Assembly.GetExecutingAssembly();


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
*/
class testendpoint : ProtoEndpointBase.Request<product>.WithResponse<ProductResponse>
{
    private readonly INameingService _nameingService;

    public testendpoint(INameingService name)
    {
        _nameingService = name;
    }
    public override async Task<ProductResponse> HandleAsync(product Request)
    {
        Console.WriteLine("ITS ALIVE !!!! my ID is "+ UserId);
        var pr = new ProductResponse() { Name = _nameingService.GetnameCongrats("mahmoud"), Description = Request.Description+ "and a response from the server", Price = 10};
        return pr;
    }
}

class TestNotifications : ProtoEndpointBase.Notification<ProductResponse>
{
    private readonly INameingService _nameingService;

    public TestNotifications(INameingService name)
    {
        _nameingService = name;
    }

    public override Task<ProductResponse> HandleAsync()
    {
        throw new NotImplementedException();
    }
}
