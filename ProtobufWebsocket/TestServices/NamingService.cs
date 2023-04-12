using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
