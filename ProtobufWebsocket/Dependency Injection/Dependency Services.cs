using Microsoft.Extensions.DependencyInjection;
using ProtobufWebsocket.Websocket_Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.Dependency_Injection
{
    public static class DependencyServices
    {

        public static void AddWebsocketDependencyInjection(this IServiceCollection services)
        {
           // services.AddSingleton<IServerInstance,ServerInstance>();
        }
    }
}
