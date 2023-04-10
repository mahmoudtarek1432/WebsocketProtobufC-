using ProtobufWebsocket.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.Model
{
    [EndpointRequest]
    internal class product : IRequest
    {

        public product(string name,IRequest x) { }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public String test { get; set; }

        public int test2 { get; set; }
    }
}
