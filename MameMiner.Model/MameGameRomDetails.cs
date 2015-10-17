using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MameMiner.Model
{
    /// <summary>
    /// Mame Game Rom Details - Includes the rom name, the sha1 hash, the rom CRC, and the file size
    /// </summary>
    public class MameGameRomDetails
    {
        /// <summary>
        /// The name of the rom file - ex. pacman.6e
        /// </summary>
        public string RomName { get; private set; }

        /// <summary>
        /// The cryptographic hash of the file - ex. e87e059c5be45753f7e9f33dff851f16d6751181
        /// </summary>
        public string SHA1 { get; private set; }

        /// <summary>
        /// The 32 bit crc check of the file - ex. c1e6ab10 - but it will be stored as 3253119760
        /// </summary>
        public int CRC { get; private set; }

        /// <summary>
        /// The size of the file in bytes - ex. 4096
        /// </summary>
        public long FileSize { get; private set; }

        /// <summary>
        /// This rom is known to have a bad CRC
        /// </summary>
        public bool BadCRC { get; private set; }

        /// <summary>
        /// This rom is known to have a bad SHA
        /// </summary>
        public bool BadSHA1 { get; private set; }

        /// <summary>
        /// This rom is a bad dump
        /// </summary>
        public bool BadDump { get { return BadCRC || BadSHA1; } }

     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="romName"></param>
        /// <param name="sha1"></param>
        /// <param name="romCRC"></param>
        /// <param name="fileSize"></param>
        /// <param name="badCRC"></param>
        /// <param name="badSHA1"></param>
        public MameGameRomDetails(string romName, string sha1, int romCRC, long fileSize, bool badCRC, bool badSHA1)
        {
            RomName = romName;
            SHA1 = sha1;
            CRC = romCRC;
            FileSize = fileSize;
            BadCRC = badCRC;
            BadSHA1 = badSHA1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mameGameRomEntry"></param>
        public MameGameRomDetails(string mameGameRomEntry) : this("", "", int.MinValue, long.MinValue, false, false)
        {
            var tokens = mameGameRomEntry.Split(new string[] { " " },
                StringSplitOptions.RemoveEmptyEntries);

            // This is the hex string of the CRC, used for elimination to find the file name.
            var longCRC = "";

            // Flag it early if it's a bad CRC or a bad SHA1
            if (mameGameRomEntry.Contains("BAD CRC"))
                BadCRC = true;

            if (mameGameRomEntry.Contains("BAD SHA1"))
                BadSHA1 = true;

            // look for SHA1 and CRC values
            foreach (var t in tokens)
            {
                if (t.Contains("SHA"))
                {
                    this.SHA1 = t.Replace("SHA1(", "").Replace(")", "");
                }

                if (t.Contains("CRC"))
                {
                    longCRC = t.Replace("CRC(", "").Replace(")", "");
                    this.CRC = int.Parse(t.Replace("CRC(", "").Replace(")", ""), System.Globalization.NumberStyles.HexNumber);
                }
            }

            // A messy hack to try and find the actual file name.
            RomName = mameGameRomEntry.Replace("   ", "|").Replace("  ", "|").Split('|')[0];

            // Now that we have the rom name, the SHA Value, etc, we can strip everything out of the
            // string, and everything remaining should be the file size.
            string fs = mameGameRomEntry.Replace(RomName, "")
                .Replace("BAD DUMP", "")
                .Replace("BAD CRC", "").Replace("CRC", "").Replace("(" + longCRC + ")", "")
                .Replace("BAD SHA1", "").Replace("SHA1", "").Replace("(" + SHA1 + ")", "");

            long outFSVal;

            // If there is anything left, it HAS to be the file size.
            if (long.TryParse(fs, out outFSVal))
                FileSize = outFSVal;
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public MameGameRomDetails() : this("", "", int.MinValue, long.MinValue, false, false) { }

    }
}
