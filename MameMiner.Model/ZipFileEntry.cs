using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MameMiner.Model
{
    /// <summary>
    /// This describes the contents of a zip file entry
    /// </summary>
    public class ZipFileEntry
    {
        /// <summary>
        /// 
        /// </summary>
        public string ZipFileContainer { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public string FileName { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public long FileSize { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public int CRC { get; private set; }

     

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileSize"></param>
        /// <param name="crc"></param>
        /// <param name="sha1"></param>
        public ZipFileEntry(string zipFileFileName, string fileName, long fileSize, int crc)
        {
            ZipFileContainer = zipFileFileName;
            FileName = fileName;
            FileSize = fileSize;
            CRC = crc;
       
        }

        public ZipFileEntry() : this("", "", long.MinValue, int.MinValue) { }

    }
}
