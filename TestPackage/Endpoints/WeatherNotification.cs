using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtobufWebsocket.EndpointApi;
using TestPackage.DTO;

namespace TestPackage.Endpoints
{
    internal class WeatherNotification : ProtoEndpointBase.Notification<WeatherResponse>
    {
        public override Task<WeatherResponse> Handle()
        {
            throw new NotImplementedException();
        }
    }
}
