using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MameMiner.Model.Model
{
    [Serializable]
    public class MameMinerAppSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public string PathToMameExecutable { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RomImportPath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GameExportPath { get; set; }
    }
}
