using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.IO;
using System.Runtime;
using System.Diagnostics;

namespace MameMiner
{
    public class GameData
    {

        void PopulateFullText()
        {
            var start = new ProcessStartInfo()
            {
                FileName = Properties.Settings.Default.MamePath,
                Arguments = "-listroms " + GameName,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(start))
            {

                using (StreamReader reader = process.StandardOutput)
                {
                    _fullText = reader.ReadToEnd();
                }
            };
        }

        public string[] FileNames { get; private set; }
        public string[] CRCs { get; private set; }
        public string[] SHA1s { get; private set; }

        public bool IsGood { get; private set; }

        private string _fullText;
        public string FullText
        {
            get
            {
                if (string.IsNullOrEmpty(_fullText))
                {
                    PopulateFullText();
                }

                return _fullText;
            }
        }

        public string GameName { get; private set; }
        public string GameDescription { get; private set; }

        public GameData(string gameListString)
        {
            GameName = gameListString.Split('|')[0];
            GameDescription = gameListString.Split('|')[1];


        }


    }
}
