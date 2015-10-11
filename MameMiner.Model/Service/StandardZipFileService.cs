using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Data.SQLite;

using Ionic.Zip;

namespace MameMiner.Model.Service
{
    /// <summary>
    /// 
    /// </summary>
    class StandardZipFileService : IZipFileService
    {
        const string CMDCreateTable = @"CREATE TABLE RomFileDetails( 
            ContainerPath char(255),
            RomName char(80),
            FileSize long,
            CRC32 long,
            SHA1 char(160)";

        /// <summary>
        /// An SQL Command to insert a new record into the database.
        /// </summary>
        const string CMDInsertRecord = @"INSERT INTO RomFileDetails (
            ContainerPath, 
            RomName, 
            FileSize, 
            CRC32,
            SHA1) 
            VALUES (
            @ContainerPath, 
            @RomName, 
            @FileSize, 
            @CRC32,
            @SHA1)";

        /// <summary>
        /// Search for the record using the name of the rom and crc32 of the file.
        /// </summary>
        const string CMDSearchByRomName = @"SELECT 
            ContainerPath, 
            RomName, 
            FileSize, 
            CRC32,
            SHA1
            FROM RomFileDetails WHERE RomName = @RomName";

        /// <summary>
        /// 
        /// </summary>
        private string _connectionString;

        /// <summary>
        /// 
        /// </summary>
        public StandardZipFileService()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData,
                Environment.SpecialFolderOption.Create), "mame_games.db");

            _connectionString = string.Format("Data Source={0};Version=3;", dbPath);

        }

        ///
        public void AddFileToZipFile(string zipFileName, string fileName, byte[] fileContents)
        {
            if(!File.Exists(zipFileName))
            {
                CreateZipFile(zipFileName);
            }


            var zf = ZipFile.Read(zipFileName);
            zf.AddEntry(fileName, fileContents);
            zf.Save();
        }

        public void CreateDatabase()
        {
           
        }

        public void CreateZipFile(string zipFileName)
        {
           if(File.Exists(zipFileName))
            {
                File.Delete(zipFileName);
            }

            var zf = new ZipFile(zipFileName);
            zf.Save();
        }

        public void DestroyDatabase()
        {
            throw new NotImplementedException();
        }

        public DataTable QueryDatabase(IDbCommand command)
        {
            throw new NotImplementedException();
        }

        public List<string> ReadAllFileNames()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipFileName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public byte[] ReadFile(string zipFileName, string fileName)
        {
            var zf = ZipFile.Read(zipFileName);
            var ze = zf.Where(x => x.FileName.ToLower().Contains(fileName.ToLower())).FirstOrDefault();

            var ms = new MemoryStream();

            if (ze != null)
            {
                ze.Extract(ms);

                return ms.ToArray();
            }

            return null;
        }

        public List<string> ReadZipFileTOC(string zipFileName)
        {
            throw new NotImplementedException();
        }

        public void WriteToDatabase(List<IDbCommand> commands)
        {
            throw new NotImplementedException();
        }
    }
}
