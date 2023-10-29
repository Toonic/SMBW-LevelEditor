﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Fushigi.Byml.Writer.Primitives
{
    public class BymlFloatData : BymlData
    {
        private readonly float Value;
        public BymlFloatData(float value)
        {
            Value = value;
        }

        public override BymlNodeId GetTypeCode() => BymlNodeId.Float;

        public override void Write(Stream stream)
        {
            stream.AsBinaryWriter().Write(Value);
        }
    }
}
