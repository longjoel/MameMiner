using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Data.SQLite;

using Ionic.Zip;


namespace MameMiner.Service
{
    /// <summary>
    /// 
    /// </summary>
    class StandardZipFileService : IZipFileService
    {

        /// <summary>
        /// 
        /// </summary>
        const string CMDCreateTable = @"CREATE TABLE RomFileDetails( 
            ContainerPath char(80),
            RomName char(80),
            FileSize long,
            CRC32 long)";

        /// <summary>
        /// An SQL Command to insert a new record into the database.
        /// </summary>
        const string CMDInsertRecord = @"INSERT INTO RomFileDetails (
            ContainerPath, 
            RomName, 
            FileSize, 
            CRC32) 
            VALUES (
            @ContainerPath, 
            @RomName, 
            @FileSize, 
            @CRC32)";

        /// <summary>
        /// Search for the record using the name of the rom and crc32 of the file.
        /// </summary>
        const string CMDSearchByRomName = @"SELECT 
            ContainerPath, 
            RomName, 
            FileSize, 
            CRC32         
            FROM RomFileDetails WHERE RomName = @RomName and FileSize = @FileSize and CRC32 = @CRC32";


        /// <summary>
        /// 
        /// </summary>
        private string _connectionString;

        /// <summary>
        /// 
        /// </summary>
        private string _dbPath;

        private IMameMinerSettingsService _settingsService;
        /// <summary>
        /// 
        /// </summary>
        public StandardZipFileService(IMameMinerSettingsService settingsService)
        {
            _dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData,
                Environment.SpecialFolderOption.Create), "MameMiner", "mame_games.db");

            _connectionString = string.Format("Data Source={0};Version=3;", _dbPath);

            if (!Directory.Exists(Path.GetDirectoryName(_dbPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_dbPath));
               

            }

            if(!File.Exists(_dbPath))
            {
                CreateDatabase();
            }


            _settingsService = settingsService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipFileName"></param>
        /// <param name="fileName"></param>
        /// <param name="fileContents"></param>
        public void AddFileToZipFile(string zipFileName, string fileName, byte[] fileContents)
        {
            if (!File.Exists(zipFileName))
            {
                CreateZipFile(zipFileName);
            }


            var zf = ZipFile.Read(zipFileName);
            zf.AddEntry(fileName, fileContents);
            zf.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        public void CreateDatabase()
        {
            //DestroyDatabase();

            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();

                using (var cmd = new SQLiteCommand(CMDCreateTable, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                conn.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipFileName"></param>
        public void CreateZipFile(string zipFileName)
        {
            if (File.Exists(zipFileName))
            {
                File.Delete(zipFileName);
            }

            var zf = new ZipFile(zipFileName);
            zf.Save();
        }

        /// <summary>
        /// 
        /// </summary>
        public void DestroyDatabase()
        {
            if (File.Exists(_dbPath))
                File.Delete(_dbPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipFileName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public long GetFileCRC(string zipFileName, string fileName)
        {
            var zf = ZipFile.Read(zipFileName);
            var ze = zf.Where(x => x.FileName.ToLower().Contains(fileName.ToLower())).FirstOrDefault();

            return ze.Crc;
        }

      

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipFileName"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public long GetFileSize(string zipFileName, string fileName)
        {
            var zf = ZipFile.Read(zipFileName);
            var ze = zf.Where(x => x.FileName.ToLower().Contains(fileName.ToLower())).FirstOrDefault();

            return ze.UncompressedSize;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public DataTable QueryDatabase(string romName, long crc32, long filesize)
        {
            var dt = new DataTable();

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand(CMDSearchByRomName, connection))
                {
                    cmd.Parameters.AddWithValue("@RomName", romName);
                    cmd.Parameters.AddWithValue("@CRC32", crc32);
                    cmd.Parameters.AddWithValue("@FileSize", filesize);
                    var da = new SQLiteDataAdapter(cmd).Fill(dt);
                }

                connection.Close();
            }

            return dt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> ReadAllFileNames()
        {
            return Directory.EnumerateFiles(_settingsService.GetMameImportPath(), "*.*", SearchOption.AllDirectories).ToList();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="zipFileName"></param>
        /// <returns></returns>
        public List<string> ReadZipFileTOC(string zipFileName)
        {
            return ZipFile.Read(zipFileName).Select(x => x.FileName).ToList();
        }

        void IZipFileService.WriteToDatabase(string zipFileName, string romName, long fileSize, long crc32)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();

                using (var cmd = new SQLiteCommand(CMDInsertRecord, connection))
                {
                    cmd.Parameters.AddWithValue("@ContainerPath", zipFileName);
                    cmd.Parameters.AddWithValue("@RomName", romName);
                    cmd.Parameters.AddWithValue("@FileSize", fileSize);
                    cmd.Parameters.AddWithValue("@CRC32", crc32);
               

                    cmd.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

    }
}
