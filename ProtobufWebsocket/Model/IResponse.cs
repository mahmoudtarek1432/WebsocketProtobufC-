using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.Model
{
    internal abstract class IResponse : ISerializable
    {
        public string? requestId { get; }

        public bool? resultCode { get; set; }

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
