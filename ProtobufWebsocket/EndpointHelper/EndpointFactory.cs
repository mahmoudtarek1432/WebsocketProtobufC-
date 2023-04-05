using ProtoBuf;
using ProtobufWebsocket.Assembly_Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.EndpointHelper
{
    internal class EndpointFactory
    {
        public static void ConvertIntoAnEndpoint(Type baseType, ModuleBuilder module)
        {
            //build a clone of a type into a new module
            var typebuilder = module.DefineType(baseType.Name, System.Reflection.TypeAttributes.Public); //defined a type with the same namespace 
            var contract = ProtoAssemblyBuilder.DecorateType<ProtoContractAttribute>();
            typebuilder.SetCustomAttribute(contract);
            //decorated the new class with protocontract 

            if (baseType.GetProperties() != null)
            {
                var properties = baseType.GetProperties();
                int index = 1;
                foreach (var property in properties)
                {              
                    var PropertyBuilder = property.CloneProperty(typebuilder);
                    decoratePropertiesWithProtoMember(PropertyBuilder, index++);
                }

                //properties now are members


            }
        }

        private static void decoratePropertiesWithProtoMember(PropertyBuilder property, int tag)
        {
            var member = ProtoAssemblyBuilder.DecorateType<ProtoMemberAttribute>(new Type[] { typeof(int) }, new object[] { tag });
            property.SetCustomAttribute(member);
        }
    }
}
