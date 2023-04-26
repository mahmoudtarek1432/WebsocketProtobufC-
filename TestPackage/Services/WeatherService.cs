using ProtobufWebsocket.Bidirectional_Helpers.Notification.Notification_Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestPackage.DTO;
using TestPackage.Endpoints;

namespace TestPackage.Services
{
    internal class WeatherService : IWeatherService
    {
        private readonly INotificationService _notificationService;

        public WeatherService(INotificationService notificationService) {
            _notificationService = notificationService;
        }
        public WeatherResponse GetService(WeatherRequest weather)
        {
            Console.WriteLine("Database query");
            return new WeatherResponse { request_id = weather.request_id, Name = "DatabaseName", Price = 100 };
        }

        public async void NotifyWeatherChange()
        {
            _notificationService.SendNotification<WeatherNotification>();
        }
    }
}
