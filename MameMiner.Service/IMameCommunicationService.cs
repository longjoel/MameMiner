using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MameMiner.Service
{
    /// <summary>
    /// The mame communication service wraps 
    /// </summary>
    public interface IMameCommunicationService
    {
        /// <summary>
        /// This generates a complete list of games from MAME.
        /// </summary>
        /// <returns></returns>
        string ListFull();

        /// <summary>
        /// This generates a list of roms needed for a specific game.
        /// </summary>
        /// <param name="gameName"></param>
        /// <returns></returns>
        string ListRoms(string gameName);

        /// <summary>
        /// This is helpfull for determining if there are other files needed for a given game, other than just the roms
        /// reported in listroms.
        /// </summary>
        /// <param name="gameName"></param>
        /// <returns></returns>
        string ListMedia(string gameName);
    }
}
