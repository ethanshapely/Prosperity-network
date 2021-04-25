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
    public partial class BaseGraph : UserControl
    {
        //public static readonly DependencyProperty LineProp = DependencyProperty.Register("LineValue", typeof(int), typeof(BaseGraph), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty QualityProp = DependencyProperty.Register("GraphQuality", typeof(string), typeof(BaseGraph), new FrameworkPropertyMetadata(null));
        Quality quality;

        /*public int LineValue
        {
            get { return (int)GetValue(LineProp); }
            set { SetValue(LineProp, value); }
        }*/

        public string GraphQuality
        {
            get { return (string)GetValue(QualityProp); }
            set
            {
                SetValue(QualityProp, value);
                if (value == "Low")
                {
                    quality = Quality.Low;
                }
                else if (value == "Medium")
                {
                    quality = Quality.Medium;
                }
                else if (value == "Highest")
                {
                    quality = Quality.Highest;
                }
                else
                {
                    quality = Quality.High;
                }
            }
        }

        public BaseGraph()
        {
            SeriesCollection = new SeriesCollection { };
            XFormatter = value => value.ToString("0");
            YFormatter = value => value.ToString("0.##");

            //modifying any series values will also animate and update the chart
            //SeriesCollection[0].Values.Add(5d);

            DataContext = this;
        }

        public void AddValues(int index, double[] values)
        {
            SeriesCollection[index].Values = values.AsGearedValues().WithQuality(quality);
        }

        //Add on property change: add NoCoops to ChartValues

        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> XFormatter { get; set; }
        public Func<double, string> YFormatter { get; set; }
    }
}
