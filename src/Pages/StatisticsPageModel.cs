using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using System.Collections.ObjectModel;
using System.ComponentModel;

#pragma warning disable 8618

namespace DiffClient.Pages
{
    /// <summary>
    /// chart from @oxyplot  https://oxyplot.readthedocs.io/en/latest/models/series/BarSeries.html
    /// </summary>
    internal class StatisticsPageModel : BasePageModel<StatisticsPage>
    {
        #region Private Members

        private void addCos()
        {
            this.DiffModel = new PlotModel { Title = "Example 1" };
            this.DiffModel.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));
        }
        private void addPie()
        {
            dynamic seriesP1 = new PieSeries { StrokeThickness = 2.0, InsideLabelPosition = 0.8, AngleSpan = 360, StartAngle = 0 };
            seriesP1.Slices.Add(new PieSlice("Africa", 1030) { IsExploded = false, Fill = OxyColors.PaleVioletRed });
            seriesP1.Slices.Add(new PieSlice("Americas", 929) { IsExploded = true });
            seriesP1.Slices.Add(new PieSlice("Asia", 4157) { IsExploded = true });
            seriesP1.Slices.Add(new PieSlice("Europe", 739) { IsExploded = true });
            seriesP1.Slices.Add(new PieSlice("Oceania", 35) { IsExploded = true });
            this.DiffModel.Series.Add(seriesP1);

            var arrowAnnotation = new ArrowAnnotation
            {
                StartPoint = new DataPoint(0, 0),
                EndPoint = new DataPoint(10, 10)
            };
            this.DiffModel.Annotations.Add(arrowAnnotation);

            var textannotation = new TextAnnotation
            {
                Text = "Text",
                TextColor = OxyColor.FromRgb(23, 0x33, 0x44)
            };
            this.DiffModel.Annotations.Add(textannotation);
        }
        private void addScatter()
        {
            this.DiffModel = new PlotModel { Title = "ScatterSeries" };
            var scatterSeries = new ScatterSeries { MarkerType = MarkerType.Circle };
            var r = new Random(314);
            for (int i = 0; i < 100; i++)
            {
                var x = r.NextDouble();
                var y = r.NextDouble();
                var size = r.Next(5, 15);
                var colorValue = r.Next(100, 1000);
                scatterSeries.Points.Add(new ScatterPoint(x, y, size, colorValue));
            }

            this.DiffModel.Series.Add(scatterSeries);
            this.DiffModel.Axes.Add(new LinearColorAxis { Position = AxisPosition.Right, Palette = OxyPalettes.Jet(200) });
        }

        private void addTornado()
        {
            this.DiffModel = new PlotModel { Title = "TornadoBarSeries" };
            var tornadobarSeries = new TornadoBarSeries { };
            this.DiffModel.Series.Add(tornadobarSeries);
        }

        private void addLines()
        {
            this.DiffModel = new PlotModel { Title = "TornadoBarSeries" };
            var lineSeries1 = new LineSeries { };
            for (int i = 0; i < 20; i++)
            {
                lineSeries1.Points.Add(new DataPoint(new Random().Next(0, 100), new Random().Next(0, 100)));
            }
            this.DiffModel.Series.Add(lineSeries1);
        }

        private void addColumn()
        {
            DiffModel = new PlotModel { Title = "AreaSeries" };
            var areaSeries = new AreaSeries();
            DiffModel.Series.Add(areaSeries);
        }


        #endregion

        #region Notify Propertys

        private ObservableCollection<DiffDecompileItem> _observable;

        public ObservableCollection<DiffDecompileItem> ObservableItems
        {
            get
            {
                if (_observable == null)
                {
                    _observable = new ObservableCollection<DiffDecompileItem>();
                }
                return _observable;
            }

            set
            {
                _observable = value;
                OnPropertyChanged("ObservableItems");
            }
        }

        #endregion

        #region Public Members

        public PlotModel DiffModel { get; private set; }

        public List<DiffDecompileItem> DiffDecompileItems = new();

        public ObservableCollection<string> AutoCompleteDataSource { get; set; } = new ObservableCollection<string>();

        public string FilterCurrent { get; set; }

        public void InitData(IEnumerable<DiffDecompileItem> items)
        {
            if (_observable == null)
            {
                _observable = new ObservableCollection<DiffDecompileItem>();
            }
            foreach (var item in items)
            {
                _observable.Add(item);
                DiffDecompileItems.Add(item);
            }
        }

        #endregion

        public StatisticsPageModel(MainWindow mainWindow, StatisticsPage view) : base(mainWindow, view)
        {
            addColumn();

            AutoCompleteDataSource.Add("contains: ");
            AutoCompleteDataSource.Add("starts: ");
            AutoCompleteDataSource.Add("contents: ");
        }        
    }
}
