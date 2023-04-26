using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.Reflection_Extentions
{
    internal static class TypeBuilderExtentions
    {
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
            return tb.DefineProperty(property.Name, PropertyAttributes.None, property.PropertyType, new Type[] { });
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
