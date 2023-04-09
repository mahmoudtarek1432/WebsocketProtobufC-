using ProtobufWebsocket.Model;
using ProtobufWebsocket.EndpointHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.EndpointApi
{
    public static class ProtoEndpoint
    {
        internal class Request<R> where R : IRequest
        {
            internal abstract class WithResponse<T> : IDynamicEndpoint  where T : IResponse 
            {
                public abstract Task<T> Handle(R Request);
            }
        }
    }
}
