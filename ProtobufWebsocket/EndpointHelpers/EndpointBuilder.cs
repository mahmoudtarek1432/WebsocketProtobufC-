using ProtoBuf;
using ProtobufWebsocket.Assembly_Helpers;
using ProtobufWebsocket.Attributes;
using ProtobufWebsocket.Reflection_Extentions;
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
    internal class Endpointbuilder
    {
        //creates a new type builder that is taken from a class at compile time.
        //the type contains all the class's fields and methods, protocontract attribute is added
        //and each field gets a protomember attribute
        public TypeBuilder PrepareForProto((Type Class, string EndpointType) baseType, ModuleBuilder module)
        {
            TypeBuilder? typebuilder;

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
                    var fieldType = BuildProtoFieldType(module, property.PropertyType);
                    var fieldBuilder = typebuilder.DefineField(property.Name, fieldType, FieldAttributes.Public);
                    DecoratePropertiesWithProtoMember(fieldBuilder, index++);
                   //recursivly, if the member is not a primitive add a protocontract to the 
                }
                //properties now are ProtoMembers
            }
            return typebuilder;
        }

        private static void DecoratePropertiesWithProtoMember(FieldBuilder field, int tag)
        {
            var member = ProtoAssemblyBuilder.DecorateType<ProtoMemberAttribute>(new Type[] { typeof(int) }, new object[] { tag });
            field.SetCustomAttribute(member);

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
                DecoratePropertiesWithProtoMember(fieldBuilder, index++);
                
            }

            return endpoint;
        }

        //checks weather the type is a primitive or a class, in case of a class, recursivly prepares it for proto maping
        private Type BuildProtoFieldType( ModuleBuilder mb,Type BasePropertyType)
        {
            //recursivly, if the member is not a primitive prepare for protobuf 
            if ((BasePropertyType.Name == "String"
                || BasePropertyType.Name == "Object"
                || BasePropertyType.Name == "Dynamic"
                || !BasePropertyType.IsClass) == false) //is a created class
            {
                var checkt = PrepareForProto((BasePropertyType, ""), mb).CreateType(); //send the created type
                return checkt!;
            }
            else if (BasePropertyType.IsGenericType)//is a collection
            {
                if (BasePropertyType.isACollection())
                {
                    var GenericTypes = BasePropertyType.GetGenericArguments();
                    foreach (var listType in GenericTypes)
                    {
                        //recursivly fetch class and prepare internal fields
                        var listclass = BuildProtoFieldType(mb, listType);
                        var ListclassArray = Array.CreateInstance(listclass, 1);
                        return ListclassArray.GetType();
                    }
                }
            }
            return BasePropertyType; //the type is a primitive 
        } 
    }
}
