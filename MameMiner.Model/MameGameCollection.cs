using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MameMiner.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class MameGameCollection : IReadOnlyCollection<MameGame>
    {

        private List<MameGame> _mameGames;

        public MameGameCollection()
        {
            _mameGames = new List<MameGame>();
        }
        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<MameGame> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
