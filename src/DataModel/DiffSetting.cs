using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable 8618

namespace DiffClient.DataModel
{
    internal class DiffSetting
    {
        public string[] RemoteUrls { get; set; }

        public string CacheDirectory { get; set; }

        public string LogFile { get; set; }

        public bool IsPreviewDiffDecompile { get; set; }

        public short HistoryNumber { get; set; }

        public bool HistoryDisableFile { get; set; }

        public bool ShowDialog { get; set; }
    }
}
