using ProtoBuf;
using ProtobufWebsocket.Assembly_Helpers;
using ProtobufWebsocket.Attributes;
using ProtobufWebsocket.Model;
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
        public static TypeBuilder ConvertIntoAnEndpoint((Type Class, string EndpointType) baseType, ModuleBuilder module)
        {
            TypeBuilder typebuilder = null;
            /*//build a clone of a type into a new module
             switch (baseType.EndpointType)
             {
                 case "request":
                     typebuilder = module.DefineType(baseType.Class.Name, System.Reflection.TypeAttributes.Public,typeof(IRequest)); 
                     break;
                 case "resposne":
                     typebuilder = module.DefineType(baseType.Class.Name, System.Reflection.TypeAttributes.Public,typeof(IResponse));
                     break;
             }*/
            typebuilder = module.DefineType(baseType.Class.Name, System.Reflection.TypeAttributes.Public);
            var contract = ProtoAssemblyBuilder.DecorateType<ProtoContractAttribute>();
            typebuilder.SetCustomAttribute(contract);
            //decorated the new class with protocontract 

            if (baseType.Class.GetProperties() != null)
            {
                var properties = baseType.Class.GetProperties();
                int index = 1;
                foreach (var property in properties)
                {              
                    var fieldBuilder = typebuilder.CloneProperty(property);
                    
                    decoratePropertiesWithProtoMember(fieldBuilder, index++);
                   
                }
                //properties now are ProtoMembers
            }
            return typebuilder;
        }

        private static void decoratePropertiesWithProtoMember(FieldBuilder field, int tag)
        {
            var member = ProtoAssemblyBuilder.DecorateType<ProtoMemberAttribute>(new Type[] { typeof(int) }, new object[] { tag });
            field.SetCustomAttribute(member);

        }

        public static (Type, string) identifyEndpoint (Type type)
        {

            var Reqatt = type.GetCustomAttributes().Where(A =>
                A.GetType().Name == typeof(EndpointRequestAttribute).Name);

            if(Reqatt.Any())
            {
                return (type, "request");
            }
            return (type, "response");
        }
    }
}
