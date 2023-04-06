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
        public static TypeBuilder PrepareForProto((Type Class, string EndpointType) baseType, ModuleBuilder module)
        {
            TypeBuilder typebuilder = null;

            typebuilder = module.DefineType(baseType.Class.Name, TypeAttributes.Public);
            var contract = ProtoAssemblyBuilder.DecorateType<ProtoContractAttribute>();
            typebuilder.SetCustomAttribute(contract);
            //decorated the new class with protocontract 

            if (baseType.Class.GetProperties() != null)
            {
                var properties = baseType.Class.GetProperties();
                int index = 1;
                foreach (var property in properties)
                {              
                    var fieldType = buildProtoFieldType(module, property.PropertyType);
                    var fieldBuilder = typebuilder.DefineField(property.Name, fieldType, FieldAttributes.Public);
                    decoratePropertiesWithProtoMember(fieldBuilder, index++);
                   //recursivly, if the member is not a primitive add a protocontract to the 
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

        public static TypeBuilder CreateEnumerableContainer(ModuleBuilder moduleBuilder, IEnumerable<Type> memberType, string name)
        {
            var endpoint = moduleBuilder.DefineType(name, TypeAttributes.Public);
            var protoContract = ProtoAssemblyBuilder.DecorateType<ProtoContractAttribute>();
            endpoint.SetCustomAttribute(protoContract);
            int index = 1;
            foreach(var Type in memberType)
            {
                var arr = Array.CreateInstance(Type, 1);
                
                var fieldBuilder = endpoint.DefineField(Type.Name,arr.GetType(),FieldAttributes.Public);
                decoratePropertiesWithProtoMember(fieldBuilder, index++);
                
            }

            return endpoint;
        }

        //checks weather the type is a primitive or a class, in case of a class, recursivly prepares it for proto maping
        public static Type buildProtoFieldType( ModuleBuilder mb,Type BasePropertyType)
        {
            //recursivly, if the member is not a primitive prepare for protobuf 
            if ((BasePropertyType.Name == "String"
                || BasePropertyType.Name == "Object"
                || BasePropertyType.Name == "Dynamic"
                || !BasePropertyType.IsClass) == false) //is a created class
            {
                var checkt = PrepareForProto((BasePropertyType, ""), mb).CreateType(); //send the created type
                return checkt;
            }
            else if (BasePropertyType.IsGenericType)//is a collection
            {
                if (BasePropertyType.isACollection())
                {
                    var GenericTypes = BasePropertyType.GetGenericArguments();
                    foreach (var listType in GenericTypes)
                    {
                        return buildProtoFieldType(mb, listType); //used to create a new 
                    }
                }
            }
            return BasePropertyType; //the type is a primitive 
        }

        
    }
}
