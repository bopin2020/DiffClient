using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

#pragma warning disable

namespace DiffClient
{
    internal enum ToolTipTarget
    {
        FileTree,
        Grouping,
        Function
    }
    internal class TreeToolTipBuilder
    {
        #region Private Members

        private StringBuilder sb = new StringBuilder();
        private string filename;
        private ToolTipTarget tiptarget;
        private string _tag;

        private string getFileDescription()
        {
            FileVersionInfo myFileVersionInfo = null;
            try
            {
                myFileVersionInfo = FileVersionInfo.GetVersionInfo(filename);
            }
            catch (Exception)
            {
                // log the specified exception
                // todo
            }
            if (myFileVersionInfo == null) { return "n/a"; }
            return myFileVersionInfo?.FileDescription;
        }

        private void build()
        {
            switch (tiptarget)
            {
                case ToolTipTarget.FileTree:
                    sb.AppendLine(getFileDescription());
                    break;
                case ToolTipTarget.Grouping:
                    break;
                case ToolTipTarget.Function:
                    sb.AppendLine(_tag);
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Public Members
        public override string ToString()
        {
            build();
            return sb.ToString();
        }

        #endregion

        public TreeToolTipBuilder(string file, ToolTipTarget target = ToolTipTarget.FileTree)
        {
            string name = file.Split('-')[0];
            string tmp = file.Split("_")[0];
            string ext = tmp.Substring(tmp.Length - 4);
            if (ext == ".sys")
            {
                file = @$"c:\windows\system32\drivers\{name}{ext}";
            }
            else 
            {
                file = @$"c:\windows\system32\{name}{ext}";
            }
            filename = file;
            tiptarget = target;
        }

        public TreeToolTipBuilder(string file,string tag, ToolTipTarget target = ToolTipTarget.FileTree)
        {
            filename = file;
            tiptarget = target;
            _tag = tag;
        }
    }
}
