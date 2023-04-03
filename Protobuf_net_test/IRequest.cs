﻿using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protobuf_net_test
{
    public class IRequest
    {
        public int request_id { get; set; }
        public bool is_subscribe { get; set; }

        public MethodType methode_type { get; set; }
    }

    public enum MethodType
    {
        HTTP = 1,
        POST = 2,
        PUT = 3,
        PATCH = 4,
        DELETE = 5,
    }
}
