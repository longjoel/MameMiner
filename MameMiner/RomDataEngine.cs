using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SQLite;

namespace MameMiner
{
    public class RomDataEngine
    {
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
        /// Search for the record using the name of the rom and SHA1 of the file.
        /// </summary>
        const string CMDSearchByRomNameAndCRC32 = @"SELECT 
            ContainerPath, 
            RomName, 
            FileSize, 
            CRC32,
            FROM RomFileDetails WHERE RomName = @RomName AND SHA1 = @SHA1";

        

        public RomDataEngine(string dbPath)
        {

        }
    }
}
