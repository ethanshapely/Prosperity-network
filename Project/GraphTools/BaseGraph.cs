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
using System.ComponentModel;
using LiveCharts;
using LiveCharts.Events;
using LiveCharts.Wpf;
using LiveCharts.Geared;

namespace Project.GraphTools
{
    public partial class BaseGraph : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty LineProp = DependencyProperty.Register("LineValues", typeof(GearedValues<double>), typeof(BaseGraph), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty QualityProp = DependencyProperty.Register("GraphQuality", typeof(Quality), typeof(BaseGraph), new FrameworkPropertyMetadata(null));
        protected Quality quality;
        GearedValues<double> values;
        //GearedValues<double> values;

        /*public int LineValue
        {
            get { return (int)GetValue(LineProp); }
            set { SetValue(LineProp, value); }
        }*/

        public GearedValues<double> LineValues
        {
            get { return (GearedValues<double>)GetValue(LineProp); }
            set 
            { 
                SetValue(LineProp, value);
                OnPropertyChanged();
                values = (GearedValues<double>)GetValue(LineProp);
            }
        }

        public Quality GraphQuality
        {
            get { return (Quality)GetValue(QualityProp); }
            set
            {
                SetValue(QualityProp, value);
                OnPropertyChanged();
                quality = (Quality)GetValue(QualityProp);
            }
        }

        public BaseGraph()
        {
            //SeriesCollection = new SeriesCollection { };
            XFormatter = value => value.ToString("0");
            YFormatter = value => value.ToString("0.##");
            values = new GearedValues<double>();

            //modifying any series values will also animate and update the chart
            //SeriesCollection[0].Values.Add(5d);

            //DataContext = this;
            //values = new GearedValues<double>().WithQuality(quality);
        }

        /*public void AddValues(int index, double[] values)
        {
            SeriesCollection[index].Values = values.AsGearedValues().WithQuality(quality);
        }*/

        //Add on property change: add NoCoops to ChartValues

        //public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> XFormatter { get; set; }
        public Func<double, string> YFormatter { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(PropertyChangedEventArgs e = null)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}
