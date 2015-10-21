using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MameMiner.Service;
using MameMiner.Model;

namespace MameMiner.Repository
{
    /// <summary>
    /// s
    /// </summary>
    class MameGameRepository : IMameGameRepository
    {
        IMameCommunicationService _communicationService;

        string[] _gameList = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mameCommunicationService"></param>
        public MameGameRepository(IMameCommunicationService mameCommunicationService)
        {
            _communicationService = mameCommunicationService;


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="maxResults"></param>
        /// <returns></returns>
        public IEnumerable<MameGame> SearchForGame(string searchTerm, int maxResults)
        {
            if (_gameList == null)
            {
                var gList = _communicationService.ListFull().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                gList.RemoveAt(0);

                _gameList = gList.ToArray();
            }

            foreach (var l in _gameList.Where(g => g.ToLower().Contains(searchTerm.ToLower())).Take(maxResults))
            {
                var entries = l.Split(new string[] { "  ", "   " }, StringSplitOptions.RemoveEmptyEntries);
                var gameName = entries[0].Trim();
                var gameDescription = entries[1].Trim().Replace("\"", "");

                yield return new MameGame(gameName,
                    gameDescription,
                    _communicationService.ListRoms(gameName),
                    _communicationService.GetNumberPlayers(gameName));
            }


        }

        public IEnumerable<MameGame> SearchForGame(string searchTerm, List<int> numPlayers, int maxResults)
        {
            if (_gameList == null)
            {
                var gList = _communicationService.ListFull().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
                gList.RemoveAt(0);

                _gameList = gList.ToArray();
            }

            foreach (var l in _gameList.Where(g => g.ToLower().Contains(searchTerm.ToLower())).Take(maxResults))
            {
                var entries = l.Split(new string[] { "  ", "   " }, StringSplitOptions.RemoveEmptyEntries);
                var gameName = entries[0].Trim();
                var gameDescription = entries[1].Trim().Replace("\"", "");

                var mameGame = new MameGame(gameName,
                    gameDescription,
                    _communicationService.ListRoms(gameName),
                    _communicationService.GetNumberPlayers(gameName));

                if (numPlayers.Contains(mameGame.NumberOfPlayers))
                    yield return mameGame;
            }


        }
    }
}
