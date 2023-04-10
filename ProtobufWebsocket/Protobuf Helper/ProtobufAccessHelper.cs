﻿using ProtoBuf.Meta;
using ProtobufWebsocket.Assembly_Helpers;
using ProtobufWebsocket.Endpoint_Provider;
using ProtobufWebsocket.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProtobufWebsocket.Protobuf_Helper
{
    internal class ProtobufAccessHelper
    {
        public static byte[] Encode(object SerializeObject)
        {
            var bytes = new byte[0];
            using (var ms = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(ms, SerializeObject);
                bytes = ms.ToArray();
            }
            return bytes;
        }

        public static object Decode(Type type,byte[] bytes)
        {
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                var decoded = ProtoBuf.Serializer.Deserialize(type, ms);
                return decoded;
            }
        }


        public static object fillEndpoint<T>(T endpointObject) where T : ISerializable
        {
            var ResposneEndpointObject = createResponseEndpoint();

            var responseEndpointType = EndpointsTypeProvider.getResponseInstance(); //endpoint response singleton

            responseEndpointType.GetRuntimeFields().ToList().ForEach(field =>
            {
                if (field.FieldType.IsArray)
                {
                    //  field class                                 //cloned in runtime class with the same name
                    if (endpointObject.GetType().Name == field.FieldType.GetElementType()!.Name) //fieldtype.name will return a string in the form of listtypename + []
                    {
                        //activator when used on an array returns object of the inside list.
                        var SerializableObjectArray = field.GetValue(ResposneEndpointObject);
                        if (SerializableObjectArray == null)
                        {
                            SerializableObjectArray = Array.CreateInstance(field.FieldType.GetElementType()!, 1); // creates an array
                        }

                        var AppendedArray = RuntimeArrayHelpers.AppendDynamicArray(SerializableObjectArray, endpointObject);

                        field.SetValue(ResposneEndpointObject, AppendedArray);
                    }
                }

            });
            return ResposneEndpointObject!;
        }

        private static object? createResponseEndpoint()
        {
            var responseType = EndpointsTypeProvider.getResponseInstance();
            return Activator.CreateInstance(responseType);
        }

    }
}
