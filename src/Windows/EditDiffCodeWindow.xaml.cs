using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DiffDecompile.Core;
using static System.Net.Mime.MediaTypeNames;

namespace DiffClient.Windows
{
    public enum DataType
    {
        None,
        Decompilation,
        Disassembly,
        DisIL,
        Graph,
        Unknown
    }
    /// <summary>
    /// EditDiffCodeWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EditDiffCodeWindow : Window
    {
        private DecompileSourceType sourceType;
        private DataType dataType;
        private string _code;
        Action<string, bool> callback;
        private bool _isleft;
        public EditDiffCodeWindow(DecompileSourceType sourceType,DataType dataType, string code,Action<string,bool> callback,bool isleft = false)
        {
            InitializeComponent();
            this.sourceType = sourceType;
            this.dataType = dataType;
            _code = code;
            this.callback = callback;
            this._isleft = isleft;

            this.richtextbox.Document.Blocks.Add(new Paragraph(new Run
            {
                Text = _code,
            }));
        }

        private void Store_Click(object sender, RoutedEventArgs e)
        {
            var _run = ((Run)(this.richtextbox.Document.Blocks.FirstBlock as Paragraph).Inlines.ToArray()[0]);
            MessageBox.Show($"change size: {_run.Text.Length}");
            if (this.callback != null)
            {
                callback(_run.Text, this._isleft);
            }
            this.Close();
        }
    }
}
