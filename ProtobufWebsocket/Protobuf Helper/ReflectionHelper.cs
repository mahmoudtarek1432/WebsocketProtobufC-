using ProtobufWebsocket.Assembly_Helpers;
using ProtobufWebsocket.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.Protobuf_Helper
{
    internal static class ReflectionHelper
    {
        static IEnumerable<Type> getassemblyWithAttributeName(this System.Reflection.Assembly assembly, string AttributeName)
        {
            return AssemblyHelper.loadAssemblyTypes(assembly).fetchClasses()
                                                             .getTypesWithAttributes(Attr => Attr
                                                             .GetType().Name == AttributeName);
        }
    }
}
