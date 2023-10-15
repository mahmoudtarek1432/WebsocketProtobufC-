using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.Dependency_Injection
{
    internal class ExcutingServiceProvider
    {
        private static IServiceProvider? Instance { get; set; }

        public static IServiceProvider GetInstance() {
            if (Instance == null)
                throw new NullReferenceException("Services are not provided yet.");
            return Instance; 
        
        }

        public static void CreateInstance(IServiceProvider instance)
        {
            Instance = instance;
        }

    }
}
