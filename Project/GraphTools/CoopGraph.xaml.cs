using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Events;
using LiveCharts.Wpf;
using LiveCharts.Geared;

namespace Project.GraphTools
{
    /// <summary>
    /// Interaction logic for CoopGraph.xaml
    /// </summary>
    /*public partial class CoopGraph : UserControl
    {
        private double trend;
        public CoopGraph()
        {
            InitializeComponent();

           // DataContext = new GraphTools.DebugModel();
           values = new GearedValues<double>().WithQuality(Quality.High);
        }

        public bool run { get; set; }
        public GearedValues<double> values { get; set; }
        public double count { get; set; }
        public double currentLecture { get; set; }
        public bool isHot { get; set; }

        public void Stop()
        {
            run = false;
        }

        public void Clear()
        {
            values.Clear();
        }

        public void Read()
        {
            if(run) return;

            //lets keep in memory only the last 20000 records,
            //to keep everything running faster
            const int keepRecords = 20000;
            run = true;

            Action readFromThread = () =>
            {
                while(run)
                {
                    Thread.Sleep(1);
                    var r = new Random();
                    trend += (r.NextDouble() < 0.5 ? 1 : -1) * r.Next(0, 10) * .001;
                    //when multi threading avoid indexed calls like -> Values[0] 
                    //instead enumerate the collection
                    //ChartValues/GearedValues returns a thread safe copy once you enumerate it.
                    //TIPS: use foreach instead of for
                    //LINQ methods also enumerate the collections
                    var first = values.DefaultIfEmpty(0).FirstOrDefault();
                    if (values.Count > keepRecords - 1) { values.Remove(first); }
                    if (values.Count < keepRecords) { values.Add(trend); }
                    isHot = trend > 0;
                    count = values.Count;
                    currentLecture = trend;
                }
            };

            //2 different tasks adding a value every ms
            //add as many tasks as you want to test this feature
            //Task.Factory.StartNew(readFromThread);
            //Task.Factory.StartNew(readFromThread);
            //Task.Factory.StartNew(readFromThread);
            //Task.Factory.StartNew(readFromThread);
            //Task.Factory.StartNew(readFromThread);
            //Task.Factory.StartNew(readFromThread);
            //Task.Factory.StartNew(readFromThread);
            //Task.Factory.StartNew(readFromThread);
        }
    }*/

    public partial class CoopGraph : BaseGraph
    {
        public static readonly DependencyProperty LineProp = DependencyProperty.Register("LineValue", typeof(int), typeof(CoopGraph), new FrameworkPropertyMetadata(null));
        List<int> coopReadings;
        public int LineValue
        {
            get { return (int)GetValue(LineProp); }
            set 
            { 
                SetValue(LineProp, value);
                coopReadings.Add(value);
                AddValues(0, coopReadings.Select(x => (double)x).ToArray());
            }
        }

        public CoopGraph() : base()
        {
            coopReadings = new List<int>();
            InitializeComponent();

            SeriesCollection.Add(new LineSeries
            {
                Title = "No. Cooperators",
                Values = new ChartValues<double> { }
            });
        }


    }
    /*public partial class CoopGraph : IDisposable
    {
        public CoopGraph()
        {
            InitializeComponent();
        }

        private void Axis_OnRangeChanged(RangeChangedEventArgs eventargs)
        {
            var vm = (ScrollableViewModel)DataContext;

            var currentRange = eventargs.Range;

            if (currentRange < TimeSpan.TicksPerDay * 2)
            {
                vm.Formatter = x => new DateTime((long)x).ToString("t");
                return;
            }

            if (currentRange < TimeSpan.TicksPerDay * 60)
            {
                vm.Formatter = x => new DateTime((long)x).ToString("dd MMM yy");
                return;
            }

            if (currentRange < TimeSpan.TicksPerDay * 540)
            {
                vm.Formatter = x => new DateTime((long)x).ToString("MMM yy");
                return;
            }

            vm.Formatter = x => new DateTime((long)x).ToString("yyyy");
        }

        public void Dispose()
        {
            var vm = (ScrollableViewModel)DataContext;
            vm.Values.Dispose();
        }
    }*/
}
