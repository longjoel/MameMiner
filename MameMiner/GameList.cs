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
    /// <summary>
    /// A tool for querying 
    /// </summary>
    public class GameList
    {

        private string[] _lines;

        /// <summary>
        /// Create a new instance of game list.
        /// </summary>
        public GameList()
        {
            var start = new ProcessStartInfo()
            {
                FileName = Properties.Settings.Default.MamePath,
                Arguments = "-ll",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(start))
            {

                using (StreamReader reader = process.StandardOutput)
                {

                    _lines = reader.ReadToEnd().Replace("  \"","|").Replace("\"","").Replace("  ","").Replace(" |", "|")
                        .Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                        .Where(x => !x.Contains("Name:")).ToArray();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public List<GameData> SearchGame(string searchString)
        {
            var gameData = new List<GameData>();

            foreach(var l in _lines.Where(x => x.ToLower()
                .Contains(searchString.ToLower())).ToList())
            {
                gameData.Add(new GameData(l));
            }

            return gameData;
        }
    }
}
