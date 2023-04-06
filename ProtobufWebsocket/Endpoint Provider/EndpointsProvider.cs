using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace ProtobufWebsocket.Endpoint_Provider
{
    internal class EndpointsTypeProvider
    {
        static Type _response { get; set; }
        static Type _request { get; set; }

        public static Type getResponseInstance()
        {
            if (_response == null)
            {
                throw new ArgumentNullException(nameof(getResponseInstance));
            }
            return _response;
        }

        public static void CreateResponseEndpointSingleton(Type responses)
        {
            _response = responses;
        }


        public static Type getRequestInstance()
        {
            if (_request == null)
            {
                throw new ArgumentNullException(nameof(getRequestInstance));
            }
            return _request;
        }

        public static void CreateRequestEndpointSingleton(Type request)
        {
            _request = request;
        }
    }
}
