﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Diagnostics;

namespace MameMiner.Model.Service
{
    public class StandardMameCommunicationService : IMameCommunicationService
    {
        private string _pathToMameExecutable;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="gameName"></param>
        /// <returns></returns>
        private string Query(string options, string gameName)
        {
            string fullText;

            var start = new ProcessStartInfo()
            {
                FileName = _pathToMameExecutable,
                Arguments = string.Format("{0} {1}", options, gameName),
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = Process.Start(start))
            {

                using (StreamReader reader = process.StandardOutput)
                {
                    fullText = reader.ReadToEnd();
                }
            };

            return fullText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathToMameExecutable"></param>
        public StandardMameCommunicationService(string pathToMameExecutable)
        {
            _pathToMameExecutable = pathToMameExecutable;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ListFull()
        {
            return Query("-listfull", "");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameName"></param>
        /// <returns></returns>
        public string ListMedia(string gameName)
        {
            return Query("-listmedia", gameName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameName"></param>
        /// <returns></returns>
        public string ListRoms(string gameName)
        {
            return Query("-listroms", gameName);
        }
    }
}
