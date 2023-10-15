﻿using ProtoBuf.Meta;
using ProtobufWebsocket.Assembly_Helpers;
using ProtobufWebsocket.Attributes;
using ProtobufWebsocket.Endpoint_Provider;
using ProtobufWebsocket.EndpointHelper;
using ProtobufWebsocket.ProtoFileConstructor;
using System.Reflection;
using System.Reflection.Emit;

namespace ProtobufWebsocket.Protobuf_Helper
{
    internal class ProtobufCreationHelper
    {
        internal static void IntializeProtoEnvironment(string AssemblyName, Assembly assembly)
        {
            var module = ProtoAssemblyBuilder.DefineNewModule(AssemblyName);

            var GlobalTypes = AssemblyHelper.loadAssemblyTypes(assembly); //loads all classes at excution time
            
            //retrieves all the assemblies consuming these attributes
            var endpoints = GlobalTypes.Where(t =>
                t.GetCustomAttributes().Where(A =>
                A.GetType().Name == typeof(EndpointRequestAttribute).Name
                || A.GetType().Name == typeof(EndpointResponseAttribute).Name)
                .Any()).ToList();

            //list of tuple containing the type and a string of request or response
            var IdentifiedEndpoints = endpoints.Select(EndpointHelper.EndpointHelper.IdentifyEndpoint).ToList(); //group endpoints with thier endpoint type

            var EndpointBuilder = new List<(TypeBuilder, string)>();

            //models mapped into endpoints by adding protocontract attribute and each member gets a protomember tag
            var endpointBuilder = new Endpointbuilder();
            IdentifiedEndpoints.ForEach(E => { EndpointBuilder.Add((endpointBuilder.PrepareForProto(E, module), E.Item2)); });

            var createdTypes = EndpointBuilder.Select(Eb => (Eb.Item1.CreateType(), Eb.Item2));

            var req = CreateRequestEndpoint(module, createdTypes!);
            var res = CreateResponseEndpoint(module, createdTypes!);


            //appends them as singletons
            EndpointsTypeProvider.CreateResponseEndpointSingleton(res);
            EndpointsTypeProvider.CreateRequestEndpointSingleton(req);

            var RequestProtoFile = RuntimeTypeModel.Default.GetSchema(req, ProtoSyntax.Default);
            var ResponseProtoFile = RuntimeTypeModel.Default.GetSchema(res, ProtoSyntax.Default);

            //Console.WriteLine(RequestProtoFile + "\n");
            //Console.WriteLine(ResponseProtoFile);

            ProtoFileProvider.CreateRequestFile(RequestProtoFile);
            ProtoFileProvider.CreateResposneFile(ResponseProtoFile);
        }

        internal static Type CreateRequestEndpoint(ModuleBuilder mb, IEnumerable<(Type,string)> requests)
        {
            var reqTypes = requests.Where(T => T.Item2 == "request").Select(T => T.Item1!).ToList();
            var endpointBuilder = Endpointbuilder.CreateEnumerableContainer(mb, reqTypes,
                                                                          "RequestEndpoint");
            return endpointBuilder.CreateType()!;
        }

        internal static Type CreateResponseEndpoint(ModuleBuilder mb, IEnumerable<(Type, string)> Responses)
        {
            var resTypes = Responses.Where(T => T.Item2 == "response").Select(T => T.Item1!).ToList();
            var endpointBuilder = Endpointbuilder.CreateEnumerableContainer(mb, resTypes, "ResponseEndpoint");
            return endpointBuilder.CreateType()?? throw new ArgumentNullException();
        }
    }
}
