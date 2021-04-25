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
    /// Interaction logic for ProsperityGraph.xaml
    /// </summary>
    public partial class ProsperityGraph : BaseGraph
    {
        public static readonly DependencyProperty LineProp = DependencyProperty.Register("LineValue", typeof(int), typeof(ProsperityGraph), new FrameworkPropertyMetadata(null));
        List<double> prosperityReadings;
        public int LineValue
        {
            get { return (int)GetValue(LineProp); }
            set
            {
                SetValue(LineProp, value);
                prosperityReadings.Add(value);
                AddValues(0, prosperityReadings.ToArray());
            }
        }

        public ProsperityGraph() : base()
        {
            prosperityReadings = new List<double>();
            InitializeComponent();

            SeriesCollection.Add(new LineSeries
            {
                Title = "Network Prosperity",
                Values = new ChartValues<double> { }
            });
        }
    }
}
