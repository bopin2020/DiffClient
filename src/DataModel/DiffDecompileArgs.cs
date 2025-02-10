using DiffClient.Commands;
using DiffClient.UserControls;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#pragma warning disable 1591
#pragma warning disable 8618

namespace DiffClient.DataModel
{
    public class DiffDecompileArgs
    {
        public object Title { get; set; }

        public string Primary { get; set; }

        public string Secondary { get; set; }
    }
}

#pragma warning restore 1591
#pragma warning restore 8618