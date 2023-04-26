using System.Reflection;
using System.Reflection.Emit;


namespace ProtobufWebsocket.Assembly_Helpers
{
    internal class ProtoAssemblyBuilder
    {

        //function returns a module that will be used to encapsulate
        //the types used inside endpointDTOs
        public static ModuleBuilder DefineNewModule(string assemblyName)
        {
            var AName = new AssemblyName(assemblyName);
            var DynamicAssembly = AssemblyBuilder.DefineDynamicAssembly(AName, AssemblyBuilderAccess.Run);
            var definedModule = DynamicAssembly.DefineDynamicModule(AName.Name!);
            return definedModule;
        }

        //decorate types with a custom attribute
        public static CustomAttributeBuilder DecorateType<T>() where T: Attribute
        {
            var emptyType = new Type[] { };
            var AttrConstructor = typeof(T).GetConstructor(emptyType);
            var attBuilder = new CustomAttributeBuilder(AttrConstructor, new object[] { });
            return attBuilder;
        }

        //decorate types with a custom attribute with constructors
        public static CustomAttributeBuilder DecorateType<T>(Type[] typeConstructoArguments, object[] parameters)
        {
            var AttrConstructor = typeof(T).GetConstructor(typeConstructoArguments);
            var attBuilder = new CustomAttributeBuilder(AttrConstructor, parameters);
            return attBuilder;
        }
    }
}
