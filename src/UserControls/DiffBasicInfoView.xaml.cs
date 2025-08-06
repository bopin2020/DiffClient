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
using System.Windows.Navigation;
using System.Windows.Shapes;

#pragma warning disable

namespace DiffClient.UserControls
{
    public class BasicInfoLinkMetadata
    {
        public string Header { get; set; }

        public string MsdlLink { get; set; }

        public bool IsAssociatedCVE { get; set; } = false;

        public string[] CVEs { get; set; }
    }

    public class BasicInfoEntry
    {
        public string Header { get; set; }

        public string Filename { get; set; }

        public string Arch { get; set; }

        public string FileSize { get; set; }

        public string SHA256 { get; set; }

        public string SourcePatch { get; set; }

        public string OSVersion { get; set; }

        public string Subsystem { get; set; }

        public string SystemPath {  get; set; }

        public List<BasicInfoLinkMetadata> Operation {  get; set; }
    }

    /// <summary>
    /// DiffBasicInfo.xaml 的交互逻辑
    /// </summary>
    public partial class DiffBasicInfoView : UserControl
    {
        private DiffBasicInfoViewModel viewModel;
        private Thickness _thickness;
        public DiffBasicInfoView(BasicInfoEntry entry)
        {
            _thickness = new Thickness(5);
            InitializeComponent();
            viewModel = new DiffBasicInfoViewModel(this);
            this.DataContext = viewModel;
            viewModel.Header = entry.Header;

            FileName.Children.Add(new TextBlock { Text = entry.Filename,Margin = _thickness, HorizontalAlignment = HorizontalAlignment.Center });
            Arch.Children.Add(new TextBlock { Text = entry.Arch, Margin = _thickness, HorizontalAlignment = HorizontalAlignment.Center });
            FileSize.Children.Add(new TextBlock { Text = entry.FileSize, Margin = _thickness, HorizontalAlignment = HorizontalAlignment.Center });
            SHA256.Children.Add(new TextBlock { Text = entry.SHA256, Margin = _thickness, HorizontalAlignment = HorizontalAlignment.Center });
            SourcePatch.Children.Add(new TextBlock { Text = entry.SourcePatch, Margin = _thickness, HorizontalAlignment = HorizontalAlignment.Center });
            OSVersion.Children.Add(new TextBlock { Text = entry.OSVersion, Margin = _thickness, HorizontalAlignment = HorizontalAlignment.Center });
            Subsystem.Children.Add(new TextBlock { Text = entry.Subsystem, Margin = _thickness, HorizontalAlignment = HorizontalAlignment.Center });
            SystemPath.Children.Add(new TextBlock { Text = entry.SystemPath, Margin = _thickness, HorizontalAlignment = HorizontalAlignment.Center });

            arch_combobox.SelectedIndex = 0;
            FileSizeType.SelectedIndex = 1;

            foreach (var item in entry.Operation)
            {
                var hl = new Hyperlink();
                hl.Inlines.Add(item.Header);
                var btn = new Button()
                {
                    Content = hl,
                    Margin = _thickness,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                btn.Click += Btn_Click;
                Operation.Children.Add(btn);
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("not support");
        }
    }
}
