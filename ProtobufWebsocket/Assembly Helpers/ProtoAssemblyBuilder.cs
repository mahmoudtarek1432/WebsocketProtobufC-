using System.Reflection;
using System.Reflection.Emit;


namespace ProtobufWebsocket.Assembly_Helpers
{
    internal static class ProtoAssemblyBuilder
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

        //adds a new type builder to an existing ModuleBuilder 
        public static TypeBuilder BuildTypeClone(this ModuleBuilder mb, Type parent)
        {
            return mb.DefineType(parent.Name, TypeAttributes.Public, parent); //return a builder instance of a class that inherets from a type
        }

        /** <summary>defines a new field into a type builder, field is built  upon a property type</summary> */
        public static FieldBuilder ClonePropertyToField(this TypeBuilder tb, PropertyInfo property)
        {
            return tb.DefineField(property.Name, property.PropertyType, FieldAttributes.Public);
        }

        /** <summary>adds a new type builder to an existing ModuleBuilder</summary> */
        public static PropertyBuilder CloneProperty(this TypeBuilder tb, PropertyInfo property)
        {
            return tb.DefineProperty(property.Name, PropertyAttributes.None, property.PropertyType,new Type[] { });
        }

        /** <summary>a predicate that takes a type and checks if the passed type inherets from ienumerable or icollection</summary>*/
        public static bool isACollection(this Type type)
        {
            var definition = type.GetGenericTypeDefinition(); //access a generic type
            var interfaces = definition.GetInterfaces();
            return interfaces.Any(I => I.Name.Contains("IEnumerable")
                                || I.Name.Contains("ICollection"));
        }
    }
}
