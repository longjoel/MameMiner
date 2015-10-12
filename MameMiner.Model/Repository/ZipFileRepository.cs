using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MameMiner.Model.Model;

namespace MameMiner.Model.Repository
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
                var bits = _zipFileService.ReadFile(r.ZipFileFileName, r.FileName);
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
                var searchRomResults = SearchForRom(m.RomName);

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
                    _zipFileService.GetFileSize(zipFilePath, z),
                    _zipFileService.GetFileSHA1(zipFilePath, z));

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
            _zipFileService.WriteToDatabase(entry.ZipFileFileName, entry.FileName, entry.FileSize, entry.CRC, entry.SHA1);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="romName"></param>
        /// <param name="fileSize"></param>
        /// <param name="crc"></param>
        /// <param name="sha1"></param>
        /// <returns></returns>
        public List<ZipFileEntry> SearchForRom(string romName, long fileSize = long.MinValue, long crc = long.MinValue, string sha1 = "")
        {
            var zipFileEntries = new List<ZipFileEntry>();
            var dt = _zipFileService.QueryDatabase(romName);

            foreach(DataRow r in dt.Rows)
            {
                var zfe = new ZipFileEntry(r[0].ToString(), r[1].ToString(), (long)r[2], (long)r[3], r[4].ToString());

                bool add = true;

                if(fileSize != long.MinValue)
                {
                    if (zfe.FileSize == fileSize)
                        add = true;
                }

                if (crc != long.MinValue)
                {
                    if (zfe.CRC == crc)
                        add = true;
                }

                if(sha1 != string.Empty)
                {
                    if (zfe.SHA1.ToLower() == sha1.ToLower())
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
