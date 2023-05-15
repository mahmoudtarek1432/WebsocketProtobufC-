using ProtobufWebsocket.Extentions;


namespace ProtobufWebsocket.RequestMapping
{
    internal class RequestMappingHelper
    {
        public static void MapRequestResponseToEndpoint(Type request, Type Endpoint)
        {

            var requestClassName = request.Name;

            var map = RequestMapProvider.GetRequestMap();
            if (map.TryGetValue(requestClassName, out var endpoint))
                throw new Exception($"The request: {requestClassName} is associated with multiple endpoints");

            var parameters = Endpoint.RetriveConstructorParameters();

            var endpointProp = new EndpointTypeProperties() { EndpointType = Endpoint, EndpointConstructorParams = parameters };

            map.Add(requestClassName, endpointProp);
        }


        //provides refrence to both request and response endpoints
        public static EndpointTypeProperties GetEndpoint(Type EndpointDto)
        {
            
            var DtoClassName = EndpointDto.Name;

            var map = RequestMapProvider.GetRequestMap();

            var endpoint = map[DtoClassName];

            return endpoint;
        }
    }
}
