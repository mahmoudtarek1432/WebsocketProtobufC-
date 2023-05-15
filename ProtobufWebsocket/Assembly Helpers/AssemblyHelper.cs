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

        /**
         * used to resolve runtime tasks
         * <param name="Task">Shall be of type Task<object></param>
         */
        public static object resolveTask(object Task)
        {
            var resultProperty = Task.GetType().GetProperty("Result");
            if (resultProperty == null) // not a task
            {
                return Task;
            }
            else
            {
                return resultProperty.GetValue(Task)!;
            }
        }
    }
}
