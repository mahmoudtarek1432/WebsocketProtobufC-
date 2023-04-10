using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.RequestMapping
{
    internal class RequestMapProvider
    {
        
        public static Dictionary<string, Type> _RequestMap { get; set; }


        /**
         * returns a dictionary with a request classname as key and returns an endpoint type
         */
        public static Dictionary<string, Type> GetRequestMap()
        {
            if (_RequestMap == null)
            {
                _RequestMap = new Dictionary<string, Type>();
            }
            return _RequestMap;
        }
    }
}
