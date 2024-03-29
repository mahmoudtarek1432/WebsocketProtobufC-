﻿using ProtobufWebsocket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.Broadcast_Helper
{
    internal class BroadcastDictionaryProvider
    {
        private static Dictionary<string, List<string>> _dictionary; //key is the endpoint's name and the list contains the websockets subscribed on broadcast

        public static void CreateNewDictionaryInstance(string key)
        {

            _dictionary ??= new Dictionary<string, List<string>>();
            
            if (_dictionary.TryGetValue(key, out _)) // if true key is not valid
                throw new Exception("endpoint is already registered");
            
            _dictionary.Add(key, new List<string>());
        }

        /**
         * append subscribers. true if successful, false if not successful.
         */
        public static bool AddUserToEndpoint(string endpointKey, string UserGuid)
        {
            if (!_dictionary[endpointKey].Contains(UserGuid)) //user is not subscribed
            {
                _dictionary[endpointKey].Add(UserGuid);
                return true;
            }
            return false;
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
