using ProtobufWebsocket.Assembly_Helpers;

namespace ProtobufWebsocket.Extentions
{
    internal static class AssemblyExtentions
    {
        public static IEnumerable<Type> GetTypeWithAttributeName(this System.Reflection.Assembly assembly, string AttributeName)
        {
            return AssemblyHelper.loadAssemblyTypes(assembly).FetchClasses()
                                                             .GetTypesWithAttributes(Attr => Attr
                                                             .GetType().Name == AttributeName);
        }
    }
}
