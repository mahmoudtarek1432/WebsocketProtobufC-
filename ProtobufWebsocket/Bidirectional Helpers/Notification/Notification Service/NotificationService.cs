using ProtobufWebsocket.Dependency_Injection;
using ProtobufWebsocket.EndpointApi;
using ProtobufWebsocket.EndpointHelper;
using ProtobufWebsocket.EndpointHelpers;
using ProtobufWebsocket.Extentions;
using ProtobufWebsocket.Protobuf_Helper;
using ProtobufWebsocket.Websocket_Helper;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace ProtobufWebsocket.Bidirectional_Helpers.Notification.Notification_Service
{
    internal class NotificationService : INotificationService
    {
        public Task SendNotification<T>() where T : INotificationEndpoint
        {
            return new Task(delegate
            {
                var sessionManager = SessionInstance.getSessionManagerInstance();

                var notificationObject = CreateNotificationObject(typeof(T));
                var Result = EndpointHelper.EndpointHelper.InvokeHandler(notificationObject);
                var encode = ProtobufAccessHelper.Encode(Result);

                sessionManager.BroadcastAsync(encode, () => Console.WriteLine(""));
            });
        }

        public Task SendNotification<T>( IEnumerable<string> Id) where T : class, INotificationEndpoint
        {
            return new Task(delegate
                {
                    var sessions = GetSessionList(Id);

                    var x = typeof(T);

                    var constructorParamTypes = x.RetriveConstructorParameters();
                    var constructorParamObjects = constructorParamTypes.Select(T => DependencyInjectionHelper.IntializeWithDI(T));
                    var notificationObject = Activator.CreateInstance(x, constructorParamObjects);
                    if (notificationObject == null)
                        throw new Exception($"notification handler creation yeilds null {nameof(SendNotification)}");

                    var Result = EndpointHelper.EndpointHelper.InvokeHandler(notificationObject);
                    var encode = ProtobufAccessHelper.Encode(Result);
                    foreach (var session in sessions)
                    {
                        session.Context.WebSocket.Send(encode);
                    }
                });
        }

        private static IEnumerable<IWebSocketSession> GetSessionList(IEnumerable<string> Ids = null)
        {
            var sessions = SessionInstance.getSessionManagerInstance();
            var filtered = new List<IWebSocketSession>();
            if(Ids != null)
            {
                foreach(var id in Ids)
                {
                    filtered.Add(sessions[id]);
                }
                return filtered;
            }
            return sessions.Sessions;
        }

        public object CreateNotificationObject(Type type)
        {

            var constructorParamTypes = type.RetriveConstructorParameters();
            var constructorParamObjects = constructorParamTypes.Select(T => DependencyInjectionHelper.IntializeWithDI(T));
            var notificationObject = Activator.CreateInstance(type, constructorParamObjects);
            if (notificationObject == null)
                throw new Exception($"notification handler creation yeilds null {nameof(SendNotification)}");

            return notificationObject;
        }
    }
}
