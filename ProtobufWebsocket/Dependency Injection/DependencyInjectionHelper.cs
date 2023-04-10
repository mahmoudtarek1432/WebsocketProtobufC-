using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.Dependency_Injection
{
    internal class DependencyInjectionHelper
    {
        public static object IntializeWithDI(Type type)
        {
            var serviceProvider = ExcutingServiceProvider.GetInstance();
            var constructedService = serviceProvider.GetRequiredService(type); //services gets constructed using DI
            return constructedService;
        }
    }
}
