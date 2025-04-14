using DiffClient.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

#pragma warning disable

namespace DiffClient
{
    
    internal class ContextMenuContext
    {
        public MainWindow MainWindow { get; set; }

        public DataGrid DataGrid { get; set; }

        public Lazy<DataGridColumn> DataGridColumn
        {
            get
            {
                return new Lazy<DataGridColumn>(DataGrid.CurrentColumn);
            }
        }

        public object CommandParameter { get; set; }

        public object ViewAndViewModelContext { get; set; }

        public RichTextBox RichTextBox { get; set; }

        public Frame indexFrame { get; set; }
    }

    internal class ContextMenuInjection<CMD,COMPARA>
    {
        #region Private

        private ContextMenuContext _ctx;

        #endregion

        protected ContextMenu ContextMenu { get; set; }

        public ContextMenuInjection(ContextMenuContext ctx)
        {
            _ctx = ctx;
            var ctxm = new ContextMenu();
            var icmd = Activator.CreateInstance(typeof(CMD),new object[] { _ctx.MainWindow });
            if (icmd == null)
            {
                throw new Exception($"new instance {typeof(CMD)} failed");
            }
            foreach (var item in typeof(COMPARA).GetFields())
            {
                if(item.Name == "value__") { continue; }

                var desc = item.CustomAttributes.ToArray()[0];

                ContextMenuContext cmc = new ContextMenuContext();
                // todo  deep clone
                cmc.MainWindow = _ctx.MainWindow;
                cmc.DataGrid = _ctx.DataGrid;
                cmc.ViewAndViewModelContext = _ctx.ViewAndViewModelContext;
                cmc.indexFrame = _ctx.indexFrame;
                cmc.CommandParameter = item.GetValue(null);

                ctxm.Items.Add(new MenuItem()
                {
                    Header = desc.ConstructorArguments[0].Value,
                    Command = icmd as ICommand,
                    CommandParameter = cmc
                });
            }
            ContextMenu = ctxm;
        }

        public virtual ContextMenu Register()
        {
            return ContextMenu;
        }
    }
    internal class MenuInjectConsole : ContextMenuInjection<ContextMenuCommand, ContextMenuRouteEvent>
    {
        public MenuInjectConsole(ContextMenuContext ctx) : base(ctx)
        {
        }
    }

    internal class DataGridColumnInject : ContextMenuInjection<DataGridColumnCommand, DataGridColumnRouteEvent>
    {
        public DataGridColumnInject(ContextMenuContext ctx) : base(ctx)
        {
        }
    }

    internal class ProcessInject : ContextMenuInjection<ProcessRowCommand, ProcessRowCommandRouteEvent>
    {
        public ProcessInject(ContextMenuContext ctx) : base(ctx) { }
    }

    internal class DiffTreeInject : ContextMenuInjection<DiffTreeGroupingCommand, DiffTreeGroupingCommandRouteEvent> 
    {
        public DiffTreeInject(ContextMenuContext ctx) : base(ctx) { }
    }

    internal class JobManagerInject : ContextMenuInjection<JobManagerCommand, JobManagerRouteEvent>
    {
        public JobManagerInject(ContextMenuContext ctx) : base(ctx)
        {
        }
    }

    internal class StatisticsInject : ContextMenuInjection<StatisticsCommand, StatisticsCommandRouteEvent>
    {
        public StatisticsInject(ContextMenuContext ctx) : base(ctx)
        {
        }
    }
}
