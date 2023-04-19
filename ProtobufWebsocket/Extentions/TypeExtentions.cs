using System.Reflection;

namespace ProtobufWebsocket.Extentions
{
    public static class TypeExtentions
    {
        public static IEnumerable<Type> fetchClasses(this IEnumerable<Type> type)
        {
            return type.Where(t => t.IsClass);
        }

        public static IEnumerable<Type> getTypesWithAttributes(this IEnumerable<Type> type, Func<Attribute, bool> predicate)
        {
            return type.Where((t) => t.GetCustomAttributes().FirstOrDefault(predicate) != null); // has the attribute
        }

        public static IEnumerable<Type> RetriveConstructorParameters(this Type type)
        {
            var constructors = type.GetConstructors();
            var paramsList = new List<Type>();
            if (constructors.Length == 0)
                throw new Exception($"Type {type.FullName} has no constructors");
           
            foreach ( var constructor in constructors )
            {
                var parameters = constructor.GetParameters();
                foreach ( var param in parameters)
                {
                    paramsList.Add(param.ParameterType);
                }
            }
            return paramsList;
        }
    }
}
