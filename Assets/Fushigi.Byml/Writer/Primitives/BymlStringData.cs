using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Fushigi.Byml.Writer.Primitives
{
    public class BymlStringData : BymlData
    {
        private readonly string Value;
        private readonly BymlStringTable StringTable;

        public BymlStringData(string value, BymlStringTable stringTable)
        {
            Value = value;
            StringTable = stringTable;
            StringTable.TryAdd(Value);
        }

        public override BymlNodeId GetTypeCode() => BymlNodeId.String;
        public override void Write(Stream stream)
        {
            var idx = StringTable.CalcIndex(Value);
            stream.AsBinaryWriter().Write(idx);
        }
    }
}
