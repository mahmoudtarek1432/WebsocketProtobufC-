using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.Assembly_Helpers
{
    public class AssemblyHelper
    {
        public static IEnumerable<Type> loadAssemblyTypes(System.Reflection.Assembly assembly)
        {
            return assembly.GetTypes();
        }
    }
}
