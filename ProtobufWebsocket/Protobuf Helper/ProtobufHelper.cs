using ProtobufWebsocket.Assembly_Helpers;
using ProtobufWebsocket.Attributes;
using ProtobufWebsocket.EndpointHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

            var IdentifiedEndpoints = endpoints.Select(EndpointFactory.identifyEndpoint).ToList(); //group endpoints with thier endpoint type

            var EndpointBuilder = IdentifiedEndpoints.Select(E => EndpointFactory.ConvertIntoAnEndpoint(E, module));

            Console.WriteLine();
        }
    }
}
