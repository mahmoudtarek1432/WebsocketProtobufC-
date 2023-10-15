using ProtobufWebsocket.Assembly_Helpers;
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
        public void SendNotification<T>() where T : INotificationEndpoint
        {
        
            var sessionManager = SessionInstance.GetSessionManagerInstance();

            var notificationObject = CreateNotificationObject(typeof(T));
            var task = notificationObject.InvokeHandler() ;
            var resolvedResponse = AssemblyHelper.resolveTask(task);
            var endpoint = ProtobufAccessHelper.FillEndpoint(resolvedResponse);
            var encode = ProtobufAccessHelper.Encode(endpoint);

            sessionManager.BroadcastAsync(encode, () => Console.WriteLine(""));
            
        }

        public Task SendNotification<T>( IEnumerable<string> Id) where T : class, INotificationEndpoint
        {
            return Task.Run(delegate
                {
                    var sessions = GetSessionList(Id);

                    var x = typeof(T);

                    var constructorParamTypes = x.RetriveConstructorParameters();
                    var constructorParamObjects = constructorParamTypes.Select(T => DependencyInjectionHelper.IntializeWithDI(T));

                    var notificationObject = Activator.CreateInstance(x, constructorParamObjects) ?? throw new Exception($"notification handler creation yeilds null {nameof(SendNotification)}");

                    //push user id
                    //notificationObject = EndpointHelper.EndpointHelper.PassUserId(notificationObject!, "broadcast");

                    var Result = notificationObject.InvokeHandler();
                    var encode = ProtobufAccessHelper.Encode(Result);
                    foreach (var session in sessions)
                    {
                        session.Context.WebSocket.Send(encode);
                    }
                });
        }

        private static IEnumerable<IWebSocketSession> GetSessionList(IEnumerable<string>? Ids = null)
        {
            var sessions = SessionInstance.GetSessionManagerInstance();
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

            IEnumerable<Type> constructorParamTypes = type.RetriveConstructorParameters() as IEnumerable<Type>;
            object notificationObject;
            
            if(constructorParamTypes.Any())
            {
                var constructorParamObjects = constructorParamTypes.Select(T => DependencyInjectionHelper.IntializeWithDI(T)).ToArray();
                notificationObject = Activator.CreateInstance(type, constructorParamObjects)!;
            }
            else
            {
                notificationObject = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(type);
            }
            
            if (notificationObject == null)
                throw new Exception($"notification handler creation yeilds null {nameof(SendNotification)}");

            return notificationObject;
        }
    }
}
