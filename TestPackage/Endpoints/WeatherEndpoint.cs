using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtobufWebsocket.EndpointApi;
using ProtobufWebsocket.Bidirectional_Helpers.Notification.Notification_Service;
using TestPackage.DTO;
using TestPackage.Services;

namespace TestPackage.Endpoints
{
    internal class WeatherEndpoint : ProtoEndpointBase.Request<WeatherRequest>.WithResponse<WeatherResponse>
    {
        private readonly IWeatherService _weatherService;
        private readonly INotificationService _notificationService;

        public WeatherEndpoint(IWeatherService weatherService,INotificationService notificationService) {
            _weatherService = weatherService;
            _notificationService = notificationService?? throw new ArgumentException(); 
        }

        public override async Task<WeatherResponse> Handle(WeatherRequest Request)
        {
            var resposne =  _weatherService.GetService(Request);
            _weatherService.NotifyWeatherChange();
            return resposne;
        }
    }
}
