using ProtobufWebsocket.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.RequestMapping
{
    internal class RequestMappingHelper
    {
        public static void MapRequestToEndpoint(Type request, Type Endpoint)
        {
            var predicat = request.CustomAttributes.Where(A =>
                A.GetType().Name == typeof(EndpointRequestAttribute).Name);
            if (!predicat.Any())
                throw new Exception($"the passed type is not a request {MapRequestToEndpoint}");

            var requestClassName = request.GetType().Name;

            var map = RequestMapProvider.GetRequestMap();

            if (!(map[requestClassName] != null))
                throw new Exception($"The request: {requestClassName} is associated with multiple endpoints");

            map.Add(requestClassName, Endpoint);
        }

        public static Type GetEndpoint(Type request)
        {
            var predicat = request.CustomAttributes.Where(A =>
                A.GetType().Name == typeof(EndpointRequestAttribute).Name);
            if (!predicat.Any())
                throw new Exception($"the passed type is not a request {MapRequestToEndpoint}");

            var requestClassName = request.GetType().Name;

            var map = RequestMapProvider.GetRequestMap();

            var endpoint = map[requestClassName];

            return endpoint;
        }
    }
}
