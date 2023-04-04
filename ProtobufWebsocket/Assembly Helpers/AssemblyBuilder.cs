using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.Assembly_Helpers
{
    internal class ProtoAssemblyBuilder
    {

        //function returns a module that will be used to encapsulate
        //the types used inside endpointDTOs
        public static ModuleBuilder DefineNewModule(string assemblyName)
        {
            var AName = new System.Reflection.AssemblyName(assemblyName);
            var DynamicAssembly = AssemblyBuilder.DefineDynamicAssembly(AName, AssemblyBuilderAccess.Run);
            var definedModule = DynamicAssembly.DefineDynamicModule(AName.Name!);
            return definedModule;
        }

        public static CustomAttributeBuilder DecorateType<T>(T Att) where T: Attribute, new()
        {
            var emptyType = new Type[] { };
            var AttrConstructor = typeof(T).GetConstructor(emptyType);
            var attBuilder = new CustomAttributeBuilder(AttrConstructor, new object[] { });
            return attBuilder;
        }

        public static TypeBuilder BuildType(ModuleBuilder mb, Type parent)
        {
            return mb.DefineType(parent.Name,System.Reflection.TypeAttributes.Public,parent); //return a builder instance of a class that inherets from a type
        }


    }
}
