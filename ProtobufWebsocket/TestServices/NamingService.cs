namespace ProtobufWebsocket.TestServices
{
    internal class NamingService : INameingService
    {
        public string GetnameCongrats(string Name)
        {
            return $"hello {Name}, congrats on achiving it";
        }
    }
}
