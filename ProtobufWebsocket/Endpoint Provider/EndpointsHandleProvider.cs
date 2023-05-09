using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.Endpoint_Provider
{
    internal class EndpointsHandleProvider
    {
        static List<Type>? Handlers { get; set; }

        public static IEnumerable<Type> GetEndpointHandlersType()
        {
            if (Handlers == null)
            {
                throw new ArgumentNullException(nameof(GetEndpointHandlersType));
            }
            return Handlers;
        }

        public static void CreateEndpointHandlerSingleton(List<Type> handlers)
        {
            Handlers = handlers;
        }
    }
}
