using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.IO;

namespace MameMiner.Service
{

    /// <summary>
    /// 
    /// </summary>
    class MameMinerSettingsService : IMameMinerSettingsService
    {
        MameMinerAppSettings _settings;

        private string _settingsPath;

        /// <summary>
        /// 
        /// </summary>
        public MameMinerSettingsService()
        {
            _settingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData, 
                Environment.SpecialFolderOption.Create), "settings.xml");

            LoadMameMinerSettings();
        }

        public string GetMameExecutablePath()
        {
            LoadMameMinerSettings();
            return _settings.PathToMameExecutable;
        }

        public string GetMameExportPath()
        {
            LoadMameMinerSettings();
            return _settings.GameExportPath;
        }

        public string GetMameImportPath()
        {
            LoadMameMinerSettings();
            return _settings.RomImportPath;
        }

        /// <summary>
        /// 
        /// </summary>
        void LoadMameMinerSettings()
        {
            if(!File.Exists(_settingsPath))
            {
                _settings = new MameMinerAppSettings();
            }
            else
            {
                var sx = new XmlSerializer(typeof(MameMinerAppSettings));
                var sr = new StringReader(File.ReadAllText(_settingsPath));

                _settings = (MameMinerAppSettings)sx.Deserialize(sr);

            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        void SaveMameMinerSettings()
        {
            var sx = new XmlSerializer(typeof(MameMinerAppSettings));
            var sw = new StringWriter();

            sx.Serialize(sw, _settings);

            File.WriteAllText(_settingsPath, sw.ToString());

        }

        public void SetMameExecutablePath(string p)
        {
            _settings.PathToMameExecutable = p;
            SaveMameMinerSettings();

        }

        public void SetMameExportPath(string p)
        {
            _settings.GameExportPath = p;
            SaveMameMinerSettings();
        
        }

        public void SetMameImportPath(string p)
        {
            _settings.RomImportPath = p;
            SaveMameMinerSettings();
        }

    }
}
