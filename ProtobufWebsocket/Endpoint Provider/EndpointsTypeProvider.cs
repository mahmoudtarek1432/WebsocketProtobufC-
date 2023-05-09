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
        static Type? Response { get; set; }
        static Type? Request { get; set; }

        public static Type GetResponseInstance()
        {
            if (Response == null)
            {
                throw new ArgumentNullException(nameof(GetResponseInstance));
            }
            return Response;
        }

        public static void CreateResponseEndpointSingleton(Type responses)
        {
            Response = responses;
        }


        public static Type GetRequestInstance()
        {
            if (Request == null)
            {
                throw new ArgumentNullException(nameof(GetRequestInstance));
            }
            return Request;
        }

        public static void CreateRequestEndpointSingleton(Type request)
        {
            Request = request;
        }
    }
}
