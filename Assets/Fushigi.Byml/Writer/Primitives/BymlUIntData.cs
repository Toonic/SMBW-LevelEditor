using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Fushigi.Byml.Writer.Primitives
{
    public class BymlUIntData : BymlData
    {
        private readonly uint Value;
        public BymlUIntData(uint value)
        {
            Value = value;
        }

        public override BymlNodeId GetTypeCode() => BymlNodeId.UInt;

        public override void Write(Stream stream)
        {
            stream.AsBinaryWriter().Write(Value);
        }
    }
}
