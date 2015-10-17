using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace MameMiner.Service
{
    public interface IZipFileService
    {
        
        /// <summary>
        /// 
        /// </summary>
        void CreateDatabase();

        /// <summary>
        /// 
        /// </summary>
        void DestroyDatabase();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        DataTable QueryDatabase(string romName, int crc32, long filesize);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commands"></param>
        void WriteToDatabase(string zipFileName, string romName, long fileSize, int crc32);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        List<string> ReadAllFileNames();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipFileName"></param>
        /// <returns></returns>
        List<string> ReadZipFileTOC(string zipFileName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipFileName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        byte[] ReadFile(string zipFileName, string fileName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipFileName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        int GetFileCRC(string zipFileName, string fileName);

      

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipFileName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        long GetFileSize(string zipFileName, string fileName);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipFileName"></param>
        void CreateZipFile(string zipFileName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipFileName"></param>
        /// <param name="fileName"></param>
        /// <param name="fileContents"></param>
        void AddFileToZipFile(string zipFileName, string fileName, byte[] fileContents);
    }
}
