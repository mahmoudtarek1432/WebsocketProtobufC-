using ProtobufWebsocket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.Broadcast_Helper
{
    internal class broadcastDictionaryProvider
    {
        private static Dictionary<string, List<string>> _dictionary; //key is the endpoint's name and the list contains the websockets subscribed on broadcast

        public static void CreateNewDictionaryInstance(string key)
        {
            if (_dictionary == null)
            {
                _dictionary = new Dictionary<string, List<string>>();
            }
            if (_dictionary.TryGetValue(key, out _)) // if true key is not valid
                throw new Exception("endpoint is already registered");
            
            _dictionary.Add(key, new List<string>());
        }

        public static void AddUserToEndpoint(string endpointKey, string UserGuid)
        {
            _dictionary[endpointKey].Add(UserGuid);
        }

        public static List<string> GetEndpointUsers(string endpointKey)
        {
            _dictionary.TryGetValue(endpointKey, out var list);
            if (list == null)
                throw new Exception("endpoint is not created");
            return list;
        }

        public static void RemoveUserfromEndpoint(string endpointKey, string UserGuid)
        {
            _dictionary[endpointKey].Remove(UserGuid);
        }
    }
}
