using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Fushigi.Byml.Writer
{
    public class BymlPathArray
    {
        private BymlPathArrayNode Impl;

        public BymlPathArray(BymlPathArrayNode impl)
        {
            Impl = impl;
        }

        private int Count() => Impl.Arrays.Sum(x => x.Length);

        public int CalcContentSize()
        {
            return Impl.Arrays.Sum(x => x.Length * Marshal.SizeOf(typeof(BymlPathPoint)));
        }

        public int CalcHeaderSize()
        {
            return 8 + (4 * Count());
        }

        public int CalcPackSize()
        {
            if (Count() == 0)
                return 0;

            return CalcHeaderSize() + CalcContentSize();
        }

        public bool IsEmpty() => Count() == 0;


        public void Write(Stream stream)
        {
            /* Don't write if there's nothing to write. */
            if (IsEmpty())
                return;

            var writer = stream.AsBinaryWriter();
            writer.Write((byte)BymlNodeId.StringTable);
            writer.WriteUInt24((uint)Count());

            int offset = CalcHeaderSize();
            foreach (var array in Impl.Arrays)
            {
                writer.Write(offset);

                var length = array.Length * Marshal.SizeOf(typeof(BymlPathPoint));
                offset += length;
            }
            /* Write ending offset. */
            writer.Write(offset);

            foreach (var array in Impl.Arrays)
            {
                stream.WriteArray<BymlPathPoint>(array);
            }

            /* Align by 4 bytes. */
            while (stream.Position % 4 != 0)
                stream.Position++;
        }
    }
}
