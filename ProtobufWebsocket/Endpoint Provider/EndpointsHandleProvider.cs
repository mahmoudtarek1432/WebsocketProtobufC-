using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.Endpoint_Provider
{
    internal class EndpointsHandleProvider
    {
        static Type[] _Handlers { get; set; }

        public static Type[] getEndpointHandlersType()
        {
            if (_Handlers == null)
            {
                throw new ArgumentNullException(nameof(getEndpointHandlersType));
            }
            return _Handlers;
        }

        public static void CreateEndpointHandlerSingleton(Type[] handlers)
        {
            _Handlers = handlers;
        }
    }
}
