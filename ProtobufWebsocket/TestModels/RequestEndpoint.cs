using ProtoBuf;

namespace ProtobufWebsocket.TestModels
{
    [ProtoContract]
    internal class RequestEndpoint
    {
        [ProtoMember(1)]
        public List<product> product { get; set; }
        

    }  
    [ProtoContract]
    class product
        {
            [ProtoMember(1)]
            public string Name { get; set; }
        [ProtoMember(2)]
            public string Description { get; set; }
        [ProtoMember(3)]
            public decimal Price { get; set; }

        [ProtoMember(4)]
            public String test { get; set; }

        [ProtoMember(5)]
            public int test2 { get; set; }
        }
}
