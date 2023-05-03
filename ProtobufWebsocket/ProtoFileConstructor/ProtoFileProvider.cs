using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.ProtoFileConstructor
{
    internal class ProtoFileProvider
    {
        private static string RequestFile;
        private static string ResponseFile;

        public static string getRequestFile()
        {
            if (RequestFile == null)
                throw new ArgumentNullException(nameof(getRequestFile));
            return RequestFile; 
        }

        public static string getResponseFile()
        {
            if (RequestFile == null)
                throw new ArgumentNullException(nameof(getRequestFile));
            return RequestFile;
        }

        public static void createRequestFile(string requestProto)
        {
            RequestFile = requestProto;
        }

        public static void createResposneFile(string responseProto)
        {
            RequestFile = responseProto;
        }
    }
}
