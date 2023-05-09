using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.ProtoFileConstructor
{
    internal class ProtoFileProvider
    {
        private static string? RequestFile;
        private static string? ResponseFile;

        public static string GetRequestFile()
        {
            if (RequestFile == null)
                throw new ArgumentNullException(nameof(GetRequestFile));
            return RequestFile; 
        }

        public static string GetResponseFile()
        {
            if (ResponseFile == null)
                throw new ArgumentNullException(nameof(GetResponseFile));
            return ResponseFile;
        }

        public static void CreateRequestFile(string requestProto)
        {
            RequestFile = requestProto;
        }

        public static void CreateResposneFile(string responseProto)
        {
            ResponseFile = responseProto;
        }
    }
}
