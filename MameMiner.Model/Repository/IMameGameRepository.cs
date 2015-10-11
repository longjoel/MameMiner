using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MameMiner.Model.Model;

namespace MameMiner.Model.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMameGameRepository
    {
        List<MameGame> SearchForGame(string searchTerm, int maxResults);
    }
}
