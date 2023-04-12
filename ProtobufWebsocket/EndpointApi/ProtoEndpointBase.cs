using ProtobufWebsocket.Model;
using ProtobufWebsocket.EndpointHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Reflection;

namespace ProtobufWebsocket.EndpointApi
{
    public static class ProtoEndpointBase
    {
        internal class Request<R> where R : IRequest
        {
            internal abstract class WithResponse<T> : IDynamicEndpoint  where T : IResponse 
            {
                public readonly string UserId; //binds the incoming websocket id
                
                public abstract Task<T> Handle(R Request);

            }
        }

        internal abstract class Notification<Response> : IDynamicEndpoint where Response : IResponse
        {

        }
    }
}
