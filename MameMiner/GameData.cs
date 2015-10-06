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

        void PopulateAttributes()
        {
            var ftLines = FullText.Split(new string[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries);

            _fileNames = new List<string>();
            _crc32s = new List<string>();
            _fileSizes = new List<string>();

            for (int i = 2; i < ftLines.Count(); i++) // The first 2 lines are inconsequential
            {
                if (ftLines[i].Contains("NO GOOD DUMP"))
                    continue;

                if (!ftLines[i].Contains("CRC"))
                    continue;

                if (ftLines[i].Contains("BAD"))
                {
                    ftLines[i] = ftLines[i].Replace("BAD_DUMP", "").Replace("BAD", "");
                }

                // OK Mame, why do you have spaces in your bin file names?
                // This is so evil.


                var line = ftLines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var linect = line.Count();
                var crcPos = linect - 2;
                var sizePos = linect - 3;
                var fsEndPos = linect - 4;

                var fn = line[0];
                for (int j = 1; j <= fsEndPos; j++)
                {
                    fn += " ";
                    fn += line[j];
                }

                _fileNames.Add(fn);
                _fileSizes.Add(line[sizePos]);
                _crc32s.Add(long.Parse(line[crcPos]
                    .Replace("CRC(", "")
                    .Replace(")", ""),
                    System.Globalization.NumberStyles.HexNumber).ToString());
            }


        }

        private List<string> _fileNames;
        private List<string> _fileSizes;
        private List<string> _crc32s;

        public List<string> FileNames
        {
            get
            {
                if (_fileNames == null)
                {

                    PopulateAttributes();
                }

                return _fileNames;
            }
        }

        public List<string> FileSizes
        {
            get
            {
                if (_fileSizes == null)
                {
                    PopulateAttributes();
                }

                return _fileSizes;
            }
        }

        public List<string> Crc32s
        {
            get
            {
                if (_crc32s == null)
                {
                    PopulateAttributes();
                }

                return _crc32s;
            }
        }

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
