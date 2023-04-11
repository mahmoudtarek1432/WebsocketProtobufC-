using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.TestModels
{
    [ProtoContract]
    internal class ResponseEndpoint
    {
        [ProtoMember(1)]
        public List<ProductResponse> ProductResponse { get; set; }


    }

    [ProtoContract]
    class ProductResponse
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
