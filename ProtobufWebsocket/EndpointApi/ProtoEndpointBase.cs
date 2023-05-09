using ProtobufWebsocket.Model;
using ProtobufWebsocket.EndpointHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Reflection;
using ProtobufWebsocket.EndpointHelpers;

namespace ProtobufWebsocket.EndpointApi
{
    public static class ProtoEndpointBase
    {
        public static class Request<R> where R : IRequest
        {
            public abstract class WithResponse<T> : IDynamicEndpoint  where T : IResponse 
            {
                public readonly string? UserId; //binds the incoming websocket id
                
                public abstract Task<T> HandleAsync(R Request);

            }
        }

        public abstract class Notification<Response> : INotificationEndpoint , IDynamicEndpoint where Response : IResponse
        {
            public abstract Task<Response> HandleAsync();

        }
    }
}
