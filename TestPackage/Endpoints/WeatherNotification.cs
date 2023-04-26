using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtobufWebsocket.EndpointApi;
using ProtobufWebsocket.Model;
using TestPackage.DTO;

namespace TestPackage.Endpoints
{
    internal class WeatherNotification : ProtoEndpointBase.Notification<WeatherResponse>
    {
        public async override Task<WeatherResponse> HandleAsync()
        {
            var response = new WeatherResponse
            {
                Price = 20,
                Name = "from notification",
                resultCode = ResultCode.Success,
            };
            return response;
        }
    }
}
