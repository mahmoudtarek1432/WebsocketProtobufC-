using ProtobufWebsocket.Attributes;
using ProtobufWebsocket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPackage.DTO
{
    [EndpointResponse]
    internal class WeatherResponse : IResponse
    {
        public string name { get; set; }

        public decimal temp { get; set; }
    }
}
