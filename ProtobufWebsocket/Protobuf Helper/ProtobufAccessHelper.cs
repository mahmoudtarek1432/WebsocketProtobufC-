using ProtobufWebsocket.Endpoint_Provider;
using ProtobufWebsocket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.Protobuf_Helper
{
    internal class ProtobufAccessHelper
    {
        public static void Encode<T>(T SerializeObject) where T : IResponse
        {
            
        }

        public static Object? createResponseEndpoint()
        {
            var responseType = EndpointsTypeProvider.getResponseInstance();
            return  Activator.CreateInstance(responseType);
        }

        public static void fillEndpoint<T>(T serializableObject) where T : ISerializable
        {
            var ResposneEndpointObject = createResponseEndpoint();

            var responseEndpointType = EndpointsTypeProvider.getResponseInstance();

            var fields = responseEndpointType.GetRuntimeFields(); //includes lists of IResponses

            
        }
    }
}
