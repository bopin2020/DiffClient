using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffClient.DataModel
{
    internal enum DispatchEvent
    {
        CleanTablItems = 0,
        ExitApp = 1,
        OpenSetting = 2,
        AccessCloud = 3,
        OpenLog = 4,
        Dynamic = 5,
        JobManager = 6,
        AccessLocalStore = 7,
        Restart = 8,
        ClearHistories = 9,
    }
}
