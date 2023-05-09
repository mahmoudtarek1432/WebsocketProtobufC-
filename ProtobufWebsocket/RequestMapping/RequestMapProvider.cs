namespace ProtobufWebsocket.RequestMapping
{
    internal class RequestMapProvider
    {
        
        public static Dictionary<string, EndpointTypeProperties>? RequestMap { get; set; }


        /**
         * returns a dictionary with a request classname as key and returns an endpoint type
         */
        public static Dictionary<string, EndpointTypeProperties> GetRequestMap()
        {
            RequestMap ??= new Dictionary<string, EndpointTypeProperties>();

            return RequestMap;
        }
    }

    //bundles the endpoint type and extracted properties such as constructor types for preprocessing 
    //and avoiding traversing the type each time it gets invoked
    internal class EndpointTypeProperties
    {
        public Type? EndpointType { get; set; }

        public IEnumerable<Type>? EndpointConstructorParams { get; set; }
    }
}
