
namespace ProtobufWebsocket.Model
{
    public abstract class IResponse : ISerializable
    {
        public int? Request_id { get; set; }

        public ResultCode? Result_code { get; set; }

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
        public string? Message { get; set; }
    }
   
}
