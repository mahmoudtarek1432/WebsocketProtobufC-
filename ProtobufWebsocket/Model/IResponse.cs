using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.Model
{
    public abstract class IResponse : ISerializable
    {
        public string? requestId { get; }

        public ResultCode? resultCode { get; set; }

        public IEnumerable<Error>? Errors { get; set; }
    }

    public enum ResultCode
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
