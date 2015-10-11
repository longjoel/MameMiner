using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MameMiner.Model.Model;

namespace MameMiner.Model.Service
{
    public interface IMameMinerSettingsService
    {
        void SaveMameMinerSettings();
        void LoadMameMinerSettings();

        MameMinerAppSettings GetSettings();
    }
}
