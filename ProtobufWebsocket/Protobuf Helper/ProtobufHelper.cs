using ProtoBuf.Meta;
using ProtobufWebsocket.Assembly_Helpers;
using ProtobufWebsocket.Attributes;
using ProtobufWebsocket.EndpointHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProtobufWebsocket.Protobuf_Helper
{
    public class ProtobufHelper
    {
        public static void IntializeProtoEnvironment(string AssemblyName, Assembly assembly)
        {

            var GlobalTypes = AssemblyHelper.loadAssemblyTypes(assembly); //loads all classes at excution time
            var module = ProtoAssemblyBuilder.DefineNewModule(AssemblyName);

            //retrieves all the assemblies consuming these attributes
            var endpoints = GlobalTypes.Where(t =>
                t.GetCustomAttributes().Where(A =>
                A.GetType().Name == typeof(EndpointRequestAttribute).Name
                || A.GetType().Name == typeof(EndpointResponseAttribute).Name)
                .Count() > 0).ToList();

            //list of tuple containing the type and a string of request or response
            var IdentifiedEndpoints = endpoints.Select(EndpointFactory.identifyEndpoint).ToList(); //group endpoints with thier endpoint type

            var EndpointBuilder = new List<(TypeBuilder, string)>();

            //models mapped into endpoints by adding protocontract attribute and each member gets a protomember tag
            IdentifiedEndpoints.ForEach(E => { EndpointBuilder.Add((EndpointFactory.PrepareForProto(E, module), E.Item2)); });

            var createdTypes = EndpointBuilder.Select(Eb => (Eb.Item1.CreateType(), Eb.Item2));

            var reqTypes = createdTypes.Where(T => T.Item2 == "request").Select(T => T.Item1!).ToList();
            var ReqEndpoint = EndpointFactory.CreateEnumerableContainer(module, reqTypes,
                                                                          "RequestEndpoint");

            var resTypes = createdTypes.Where(T => T.Item2 == "response").Select(T => T.Item1!).ToList();
            var ResEndpoint = EndpointFactory.CreateEnumerableContainer(module, resTypes,
                                                                          "ResponseEndpoint");

            var req = ReqEndpoint.CreateType();
            var res = ResEndpoint.CreateTypeInfo();

            var proto1 = RuntimeTypeModel.Default.GetSchema(req, ProtoSyntax.Default);
            var proto2 = RuntimeTypeModel.Default.GetSchema(res, ProtoSyntax.Default);
            /*Console.WriteLine(proto1 + "\n");
            Console.WriteLine(proto2);*/
        }
    }
}
