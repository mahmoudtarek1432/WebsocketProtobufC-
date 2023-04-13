using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtobufWebsocket.EndpointApi;
using TestPackage.Models;

namespace TestPackage.Endpoints
{
    internal class WeatherEndpoint : ProtoEndpointBase.Request<WeatherRequest>.WithResponse<WeatherResponse>
    {

    }
}
