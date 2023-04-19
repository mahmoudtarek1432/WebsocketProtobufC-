using ProtobufWebsocket.Attributes;

namespace ProtobufWebsocket.Model
{
    [EndpointResponse]
    internal class ProductResponse : IResponse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
    }
}
