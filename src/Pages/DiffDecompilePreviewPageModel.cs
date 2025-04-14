using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable

namespace DiffClient.Pages
{
    internal class DiffDecompilePreviewPageModel : BasePageModel<DiffDecompilePreviewPage>
    {
        #region Private Members

        private DiffDecompilePreviewPage _diffDecompilePreviewPage;

        #endregion

        #region Public Members
        // todo register data source dynamically
        public string[] BackendSources { get; set; } = new string[] { "IDA", "Angr", "Ghidra", "BinaryNinja" };
        public string BackendSourceCurrent { get; set; } = "IDA";
        public string[] DataForms { get; set; } = new string[] { "Decompilation", "Disassembly", "IL", "Graph" };
        public string DataFormCurrent { get; set; } = "Decompilation";

        #endregion

        public DiffDecompilePreviewPageModel(MainWindow mainWindow, DiffDecompilePreviewPage view) : base(mainWindow, view)
        {
            _diffDecompilePreviewPage = view;
        }
    }

    public enum BackendSourcesType
    {
        IDA,
        Angr,
        Ghidra,
        BinaryNinja,
    }

    public enum DataFormsType
    {
        Decompilation,
        Disassembly,
        IL,
        Graph
    }

}
