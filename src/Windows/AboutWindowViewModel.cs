﻿using DiffClient.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffClient.Windows
{
    internal class AboutWindowViewModel : BaseWindowNotifyModel
    {
        #region Private Members

        private AboutWindow _aboutWindow;

        #endregion

        #region Public Members
        public string DiffClientDescription { get; set; } = $"{Const.Name} {Const.Version} {Const.Author}";
        public string DiffClientLinks { get; set; } = "https://github.com/bopin2020/DiffClient";

        #endregion

        public AboutWindowViewModel(AboutWindow aboutWindow)
        {
            _aboutWindow = aboutWindow;
        }
    }
}
