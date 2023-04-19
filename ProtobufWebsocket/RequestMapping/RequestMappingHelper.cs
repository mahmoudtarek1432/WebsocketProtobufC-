using ProtobufWebsocket.Extentions;


namespace ProtobufWebsocket.RequestMapping
{
    internal class RequestMappingHelper
    {
        public static void MapRequestToEndpoint(Type request, Type Endpoint)
        {

            var requestClassName = request.Name;

            var map = RequestMapProvider.GetRequestMap();
            if (map.TryGetValue(requestClassName, out var endpoint))
                throw new Exception($"The request: {requestClassName} is associated with multiple endpoints");

            var parameters = Endpoint.RetriveConstructorParameters();

            var endpointProp = new EndpointTypeProperties() { EndpointType = Endpoint, EndpointConstructorParams = parameters };

            map.Add(requestClassName, endpointProp);
        }

        public static EndpointTypeProperties GetEndpoint(Type request)
        {
            
            var requestClassName = request.Name;

            var map = RequestMapProvider.GetRequestMap();

            var endpoint = map[requestClassName];

            return endpoint;
        }
    }
}
