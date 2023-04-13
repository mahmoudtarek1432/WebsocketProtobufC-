using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtobufWebsocket.EndpointApi;
using TestPackage.DTO;
using TestPackage.Services;

namespace TestPackage.Endpoints
{
    internal class WeatherEndpoint : ProtoEndpointBase.Request<WeatherRequest>.WithResponse<WeatherResponse>
    {
        private readonly IWeatherService _weather;

        public WeatherEndpoint(IWeatherService weatherService) {
            _weather = weatherService;
        }

        public override Task<WeatherResponse> Handle(WeatherRequest Request)
        {
            throw new NotImplementedException();
        }
    }
}
