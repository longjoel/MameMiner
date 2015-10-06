using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SQLite;

using System.Security.Cryptography;

using System.IO;

namespace MameMiner
{
    public class RomDataEngine : IDisposable
    {
        public string _connectionString;
        public SQLiteConnection _connection;

        /// <summary>
        /// An SQL Command to create the table if none exist.
        /// </summary>
        const string CMDCreateTable = @"CREATE TABLE RomFileDetails( 
            ContainerPath char(255),
            RomName char(255),
            FileSize char(25),
            CRC32 char(25))";

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
        const string CMDSearchByRomNameAndcrc32 = @"SELECT 
            ContainerPath, 
            RomName, 
            FileSize, 
            CRC32
            FROM RomFileDetails WHERE RomName = @RomName AND CRC32 = @CRC32";


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbPath"></param>
        public RomDataEngine(string dbPath)
        {
            _connectionString = string.Format("Data Source={0};Version=3;", dbPath);

            if (!File.Exists(dbPath))
            {
                _connection = new SQLiteConnection(_connectionString);
                _connection.Open();

                using (var cmd = new SQLiteCommand(CMDCreateTable, _connection))
                {
                    cmd.ExecuteNonQuery();
                }

            }
            else
            {
                _connection = new SQLiteConnection(_connectionString);
                _connection.Open();
            }


        }

        public void Dispose()
        {
            _connection.Close();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="romName"></param>
        /// <param name="crc32"></param>
        /// <returns></returns>
        public List<RomDataEngineRecord> Query(string romName, string crc32)
        {
            var results = new List<RomDataEngineRecord>();
            var dt = new DataTable();

            using (var cmd = new SQLiteCommand(CMDSearchByRomNameAndcrc32, _connection))
            {
                cmd.Parameters.AddWithValue("@RomName", romName);
                cmd.Parameters.AddWithValue("@CRC32", crc32);

                new SQLiteDataAdapter(cmd).Fill(dt);
            }
            

            foreach (DataRow r in dt.Rows)
            {
                results.Add(new RomDataEngineRecord(r[0].ToString(),
                    r[1].ToString(),
                    r[2].ToString(),
                    r[3].ToString()));
            }

            return results;
        }

        public void Insert(string containerPath, string romName, string fileSize, string crc32)
        {

            using (var cmd = new SQLiteCommand(CMDInsertRecord, _connection))
            {
                cmd.Parameters.AddWithValue("@ContainerPath", containerPath);
                cmd.Parameters.AddWithValue("@RomName", romName);
                cmd.Parameters.AddWithValue("@FileSize", fileSize);
                cmd.Parameters.AddWithValue("@CRC32", crc32);

                cmd.ExecuteNonQuery();
            }

        }
    }

}
