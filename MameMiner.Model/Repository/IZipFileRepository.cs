using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MameMiner.Model.Model;

namespace MameMiner.Model.Repository
{
    public interface IZipFileRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipFilePath"></param>
        /// <returns></returns>
        List<ZipFileEntry> GetZipFileContents(string zipFilePath);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="romName"></param>
        /// <param name="fileSize"></param>
        /// <param name="crc"></param>
        /// <param name="sha1"></param>
        /// <returns></returns>
        List<ZipFileEntry> SearchForRom(string romName, long fileSize = long.MinValue, long crc = long.MinValue, string sha1 = "");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        void InsertRom(ZipFileEntry entry);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipFilePath"></param>
        /// <param name="roms"></param>
        void ExportZipFile(string zipFilePath, List<ZipFileEntry> roms);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        List<MameGameRomDetails> FindMissingRoms(MameGame game);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        bool HasMissingRoms(MameGame game);
    }
}
