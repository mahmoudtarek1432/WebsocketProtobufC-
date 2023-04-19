using ProtobufWebsocket.Assembly_Helpers;

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
