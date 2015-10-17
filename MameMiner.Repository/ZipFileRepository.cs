using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MameMiner.Model;
using MameMiner.Service;

namespace MameMiner.Repository
{
    /// <summary>
    /// 
    /// </summary>
    class ZipFileRepository : IZipFileRepository
    {
        /// <summary>
        /// 
        /// </summary>
        Service.IZipFileService _zipFileService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipFileServce"></param>
        public ZipFileRepository(Service.IZipFileService zipFileServce)
        {
            _zipFileService = zipFileServce;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipFilePath"></param>
        /// <param name="roms"></param>
        public void ExportZipFile(string zipFilePath, List<ZipFileEntry> roms)
        {

            _zipFileService.CreateZipFile(zipFilePath);

            foreach (var r in roms)
            {
                var bits = _zipFileService.ReadFile(r.ZipFileContainer, r.FileName);
                _zipFileService.AddFileToZipFile(zipFilePath, r.FileName, bits);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public List<MameGameRomDetails> FindMissingRoms(MameGame game)
        {
            var missing = new List<MameGameRomDetails>();

            foreach (var m in game)
            {
                var searchRomResults = SearchForRom(m.RomName, m.FileSize, m.CRC);

                if (searchRomResults.Count == 0)
                {
                    missing.Add(m);
                }
            }

            return missing;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipFilePath"></param>
        /// <returns></returns>
        public List<ZipFileEntry> GetZipFileContents(string zipFilePath)
        {
            var contents = new List<ZipFileEntry>();

            var zf = _zipFileService.ReadZipFileTOC(zipFilePath);
            foreach (var z in zf)
            {
                var zfe = new ZipFileEntry(zipFilePath, z,
                    _zipFileService.GetFileSize(zipFilePath, z),
                    _zipFileService.GetFileCRC(zipFilePath, z));

                contents.Add(zfe);
            }

            return contents;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public bool HasMissingRoms(MameGame game)
        {
            foreach (var m in game)
            {
                var searchRomResults = SearchForRom(m.RomName);

                if (searchRomResults.Count == 0)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        public void InsertRom(ZipFileEntry entry)
        {
            _zipFileService.WriteToDatabase(entry.ZipFileContainer, entry.FileName, entry.FileSize, entry.CRC);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="romName"></param>
        /// <param name="fileSize"></param>
        /// <param name="crc"></param>
        /// <param name="sha1"></param>
        /// <returns></returns>
        public List<ZipFileEntry> SearchForRom(string romName, long fileSize = long.MinValue, int crc = int.MinValue)
        {
            var zipFileEntries = new List<ZipFileEntry>();
            var dt = _zipFileService.QueryDatabase(romName, crc, fileSize);

            foreach(DataRow r in dt.Rows)
            {
                var zfe = new ZipFileEntry(r[0].ToString(), r[1].ToString(), (long)r[2], (int)r[3]);

                bool add = true;

                if(fileSize != long.MinValue)
                {
                    if (zfe.FileSize == fileSize)
                        add = true;
                }

                if (crc != int.MinValue)
                {
                    if (zfe.CRC == crc)
                        add = true;
                }


                if(add)
                {
                    zipFileEntries.Add(zfe);
                }
            }

            return zipFileEntries;
        }
    }
}
