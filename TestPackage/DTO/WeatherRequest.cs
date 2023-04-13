using ProtobufWebsocket.Attributes;
using ProtobufWebsocket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestPackage.DTO
{
    [EndpointRequest]
    internal class WeatherRequest :IRequest
    {
        public string Id { get; set; }
    }
}
