using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MameMiner.Model.Service;

namespace MameMiner.Model.Repository
{
    public static class RepositoryManager
    {
        private static Dictionary<Type, object> _repositoryCache;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetInstanceOf<T>()
        {
            if (_repositoryCache == null)
                _repositoryCache = new Dictionary<Type, object>();

            var repositoryType = typeof(T);

            if (!_repositoryCache.ContainsKey(repositoryType))
            {
                if (repositoryType == typeof(IMameGameRepository))
                {
                    var s = new MameGameRepository(Service.ServiceManager.GetInstanceOf<IMameCommunicationService>());
                    _repositoryCache[repositoryType] = s;
                }
                else if (repositoryType == typeof(IZipFileRepository))
                {
                    var s = new ZipFileRepository();
                    _repositoryCache[repositoryType] = s;
                }
            }

            return (T)_repositoryCache[repositoryType];

        }
    }
}
