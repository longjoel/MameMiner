using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MameMiner.Model;

namespace MameMiner.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMameGameRepository
    {
        List<MameGame> SearchForGame(string searchTerm, int maxResults);
    }
}
