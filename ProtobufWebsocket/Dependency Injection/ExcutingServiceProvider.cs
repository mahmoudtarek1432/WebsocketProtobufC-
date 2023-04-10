using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.Dependency_Injection
{
    internal class ExcutingServiceProvider
    {
        private static IServiceProvider _instance { get; set; }

        public static IServiceProvider GetInstance() {
            if (_instance == null)
                throw new NullReferenceException("Services are not provided yet.");
            return _instance; 
        
        }

        public static void CreateInstance(IServiceProvider instance)
        {
            _instance = instance;
        }

    }
}
