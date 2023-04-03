using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protobuf_net_test
{
    [ProtoContract]
    public abstract class IResponse
    {
        [ProtoMember(1)]
        public abstract string? requestId { get; }

        [ProtoMember(2)]
        public bool? resultCode { get; set; }

        [ProtoMember(3)]
        public IEnumerable<Error>? Errors { get; set; }
    }

    enum ResultCode
    {
        Success = 200,
        NotFound = 404,
        Subscribed = 410,
    }

    public class Error
    {
        public string? message { get; set; }
    }
}
