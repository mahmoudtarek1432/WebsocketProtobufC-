using ProtobufWebsocket.EndpointHelper;
using ProtobufWebsocket.EndpointHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.Bidirectional_Helpers.Notification.Notification_Service
{
    public interface INotificationService
    {
        public void SendNotification<T>() where T : INotificationEndpoint;
        public Task SendNotification<T>(IEnumerable<string> Id) where T : class, INotificationEndpoint;
    }
}
