using ProtobufWebsocket.Endpoint_Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.EndpointHelper.ResponseEndpoint
{
    internal class ResponseEndpointHelper
    {
        private static object CreateResponseEndpoint()
        {

            var requestEndpointType = EndpointsTypeProvider.GetResponseInstance();

            var ResponseEndpointObject = Activator.CreateInstance(requestEndpointType);

            if (ResponseEndpointObject == null)
                throw new Exception($"object {nameof(ResponseEndpointObject)} was null");

            return ResponseEndpointObject;
        }
    }
}
