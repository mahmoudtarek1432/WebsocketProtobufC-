using ProtobufWebsocket.Attributes;

namespace ProtobufWebsocket.Model
{
    [EndpointRequest]
    internal class product : IRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
    }
}
