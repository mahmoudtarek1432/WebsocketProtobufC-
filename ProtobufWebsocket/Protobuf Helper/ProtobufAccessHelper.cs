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

            responseEndpointType.GetRuntimeFields().ToList().ForEach(field =>
            {
                if (field.FieldType.Name.Replace("[]", "") == serializableObject.GetType().Name) //fieldtype.name will return a string in the form of listtypename + []
                {
                    //activator when used on an array returns object of the inside list.
                }
            });
        }
    }
}
