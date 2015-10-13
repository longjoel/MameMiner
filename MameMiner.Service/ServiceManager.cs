using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MameMiner.Service
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceManager
    {
        private static Dictionary<Type, object> _serviceCache;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetInstanceOf<T>()
        {
            if (_serviceCache == null)
                _serviceCache = new Dictionary<Type, object>();

            var serviceType = typeof(T);

            if(!_serviceCache.ContainsKey(serviceType))
            {
                if(serviceType == typeof(IMameMinerSettingsService))
                {
                    var s = new MameMinerSettingsService();
                    _serviceCache[serviceType] = s;
                }

                if(serviceType == typeof(IMameCommunicationService))
                {
                    var s = new StandardMameCommunicationService(GetInstanceOf<IMameMinerSettingsService>()
                        .GetMameExecutablePath());
                    _serviceCache[serviceType] = s;

                }

                if (serviceType == typeof(IZipFileService))
                {
                    var s = new StandardZipFileService(GetInstanceOf<IMameMinerSettingsService>());
                    _serviceCache[serviceType] = s;

                }
            }

            return (T)_serviceCache[serviceType];

        } 
    }
}
