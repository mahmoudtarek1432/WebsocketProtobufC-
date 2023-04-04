using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
    }
}
