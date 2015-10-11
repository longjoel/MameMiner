using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MameMiner.Model.Model;

using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.IO;

namespace MameMiner.Model.Service
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MameMinerAppSettings GetSettings()
        {
            return _settings;
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadMameMinerSettings()
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
        public void SaveMameMinerSettings()
        {
            var sx = new XmlSerializer(typeof(MameMinerAppSettings));
            var sw = new StringWriter();

            sx.Serialize(sw, _settings);

            File.WriteAllText(_settingsPath, sw.ToString());

        }
    }
}
