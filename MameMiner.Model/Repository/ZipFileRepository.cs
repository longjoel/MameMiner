using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MameMiner.Model.Model;

namespace MameMiner.Model.Repository
{
    class ZipFileRepository : IZipFileRepository
    {
        public void ExportZipFile(string zipFilePath, List<ZipFileEntry> roms)
        {
            throw new NotImplementedException();
        }

        public List<MameGameRomDetails> FindMissingRoms(MameGame game)
        {
            throw new NotImplementedException();
        }

        public List<ZipFileEntry> GetZipFileContents(string zipFilePath)
        {
            throw new NotImplementedException();
        }

        public bool HasMissingRoms(MameGame game)
        {
            throw new NotImplementedException();
        }

        public void InsertRom(ZipFileEntry entry)
        {
            throw new NotImplementedException();
        }

        public List<ZipFileEntry> SearchForRom(string romName, long fileSize = long.MaxValue, long crc = long.MinValue, string sha1 = "")
        {
            throw new NotImplementedException();
        }
    }
}
