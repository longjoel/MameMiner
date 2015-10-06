using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MameMiner
{
    public class RomDataEngineRecord
    {
        public string ContainerPath { get; private set; }
        public string RomName { get; private set; }
        public string FileSize { get; private set; }
        public string Crc32 { get; private set; }

        public RomDataEngineRecord(string containerPath, string romName, string fileSize, string crc32)
        {
            ContainerPath = containerPath;
            RomName = romName;
            FileSize = fileSize;
            Crc32 = crc32;

        }

        public RomDataEngineRecord() : this(string.Empty, string.Empty, string.Empty, string.Empty) { }
    }
}
