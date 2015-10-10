using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MameMiner.Model.Model
{
    public class MameGame : IReadOnlyCollection<MameGameRomDetails>
    {
        /// <summary>
        /// 
        /// </summary>
        public string GameName { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public string GameDescription { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        private List<MameGameRomDetails> _gameRomDetails;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameName"></param>
        /// <param name="gameDescription"></param>
        /// <param name="mameGameDetails"></param>
        public MameGame(string gameName, string gameDescription, string mameGameDetails)
        {
            GameName = gameName;
            GameDescription = gameDescription;

            _gameRomDetails = new List<MameGameRomDetails>();

            var lines = mameGameDetails.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Count() < 2)
                throw new FormatException("Not enough information to generate Mame Game Info.");

            for (int i = 2; i < lines.Count(); i++)
            {
                _gameRomDetails.Add(new MameGameRomDetails(lines[i]));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                return _gameRomDetails.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<MameGameRomDetails> GetEnumerator()
        {
            return _gameRomDetails.GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
