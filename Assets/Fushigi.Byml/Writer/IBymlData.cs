using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Fushigi.Byml.Writer
{
    public interface IBymlData
    {
        void MakeIndex();
        int CalcPackSize();
        BymlNodeId GetTypeCode();
        bool IsContainer();
        void Write(Stream stream);
    }
}
