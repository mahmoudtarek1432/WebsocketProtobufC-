using ProtoBuf;
using ProtoBuf.Meta;
using ProtoBuf;
using Protobuf_net_test;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;

var product = new ProductRequest
{
    Name = "Test",
    Description = "Test",
    Price = 1,
};


var model = RuntimeTypeModel.Create();
model.DefaultCompatibilityLevel = CompatibilityLevel.NotSpecified;


var proto = Serializer.GetProto<ProductRequest>();
var t = typeof (ProductRequest).IsSubclassOf(typeof(IRequest));

var An = new System.Reflection.AssemblyName("Endpoint");
var dynamicAssembly = AssemblyBuilder.DefineDynamicAssembly(An,AssemblyBuilderAccess.Run);
var definedModule = dynamicAssembly.DefineDynamicModule(An.Name);
var newtypeBuilder = definedModule.DefineType(typeof(ProductRequest).Name, System.Reflection.TypeAttributes.Public, typeof(ProductRequest));

var noAtt = newtypeBuilder.CreateType();
var instance = Activator.CreateInstance(noAtt);

var type = new Type[] { };
var att = typeof(ProtoContractAttribute).GetConstructor(type);
var ctb = new CustomAttributeBuilder(att, new object[] { });
newtypeBuilder.SetCustomAttribute(ctb);

var withatt = newtypeBuilder.CreateType();
var withattinstance = Activator.CreateInstance(withatt);

//byte[] data = new byte[5];
using (var stream = new MemoryStream())
{

    Serializer.Serialize(stream, withattinstance);
    var array = stream.ToArray();
}
var protofile = RuntimeTypeModel.Default.GetSchema(withatt, ProtoSyntax.Default);  //Serializer.GetProto<>()

Console.ReadLine();
