using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Fushigi.Byml.Writer.Primitives
{
    public class BymlNullData : BymlData
    {
        public override BymlNodeId GetTypeCode() => BymlNodeId.Null;
        public override void Write(Stream stream)
        {
            stream.AsBinaryWriter().Write((uint)0);
        }
    }
}
