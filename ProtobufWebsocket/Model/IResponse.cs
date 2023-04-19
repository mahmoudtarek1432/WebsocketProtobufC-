
namespace ProtobufWebsocket.Model
{
    public abstract class IResponse : ISerializable
    {
        public int? request_id { get; set; }

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
        public string message { get; set; }
    }
   
}
