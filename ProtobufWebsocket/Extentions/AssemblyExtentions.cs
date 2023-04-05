using ProtobufWebsocket.Assembly_Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.Extentions
{
    internal static class AssemblyExtentions
    {
        public static IEnumerable<Type> getTypeWithAttributeName(this System.Reflection.Assembly assembly, string AttributeName)
        {
            return AssemblyHelper.loadAssemblyTypes(assembly).fetchClasses()
                                                             .getTypesWithAttributes(Attr => Attr
                                                             .GetType().Name == AttributeName);
        }
    }
}
