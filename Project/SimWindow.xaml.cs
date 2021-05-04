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
using System.Windows.Shapes;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Events;
using LiveCharts.Wpf;
using LiveCharts.Geared;

namespace Project
{
    /// <summary>
    /// Interaction logic for SimWindow.xaml
    /// </summary>
    public partial class SimWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        ///private int networkCooperators;
        ///private double networkProsperity;
        ProsperityNetwork.ProsperitySimulation network;

        //public static readonly DependencyProperty QualityProperty = DependencyProperty.Register("GraphQuality", typeof(string), typeof(SimWindow), new FrameworkPropertyMetadata(null));
        //public static readonly DependencyProperty ProsperityProperty = DependencyProperty.Register("NetworkProsperity", typeof(double), typeof(SimWindow), new FrameworkPropertyMetadata(null));
        //public static readonly DependencyProperty CoopProperty = DependencyProperty.Register("NetworkNoCoops", typeof(int), typeof(SimWindow), new FrameworkPropertyMetadata(null));
        /*public DependencyProperty CoopProperty = DependencyProperty.Register("CoopTotals", typeof(GearedValues<int>), typeof(SimWindow), new FrameworkPropertyMetadata(null));
        public DependencyProperty AvrgCoopProperty = DependencyProperty.Register("AvrgCoops", typeof(GearedValues<double>), typeof(SimWindow), new FrameworkPropertyMetadata(null));
        public DependencyProperty ProsperityProperty = DependencyProperty.Register("Prosperity", typeof(GearedValues<double>), typeof(SimWindow), new FrameworkPropertyMetadata(null));
        public DependencyProperty RoleConProperty = DependencyProperty.Register("RoleConProbs", typeof(GearedValues<double>), typeof(SimWindow), new FrameworkPropertyMetadata(null));
        public DependencyProperty RoleNeighborConProperty = DependencyProperty.Register("RoleNeighborConProbs", typeof(GearedValues<double>), typeof(SimWindow), new FrameworkPropertyMetadata(null));*/
        private Quality quality;
        private Boolean paused, evolving;
        private Timer timer;
        public GearedValues<double> prosperityValues, avrgCoopsValues, avrgRoleConProbValues, avrgRoleNeighborConProbValues;
        public GearedValues<int> totalCoopValues;

        /*private string GraphQuality
        {
            get { return (string)GetValue(QualityProperty); }
        }*/

        /*private GearedValues<double> NetworkProsperity
        {
            get { return (GearedValues<double>)GetValue(ProsperityProperty); }
        }

        private GearedValues<int> NetworkCoops
        {
            get { return (GearedValues<int>)GetValue(CoopProperty); }
        }

        private GearedValues<double> NetworkAvrgCoops
        {
            get { return (GearedValues<double>)GetValue(AvrgCoopProperty); }
        }

        private GearedValues<double> NetworkRoleConProbs
        {
            get { return (GearedValues<double>)GetValue(RoleConProperty); }
        }

        private GearedValues<double> NetworkRoleNeighborConProbs
        {
            get { return (GearedValues<double>)GetValue(RoleNeighborConProperty); }
        }*/

        public GearedValues<double> Prosperity
        {
            get { return prosperityValues; }
        }

        public GearedValues<int> Cooperators
        {
            get { return totalCoopValues; }
        }

        public GearedValues<double> AverageCooperators
        {
            get { return avrgCoopsValues; }
        }

        public GearedValues<double> AverageRoleConProb
        {
            get { return avrgRoleConProbValues; }
        }

        public GearedValues<double> AverageRoleNeighborConProb
        {
            get { return avrgRoleNeighborConProbValues; }
        }

        public Quality LineQuality
        {
            get { return quality; }
        }

        public SimWindow(string simName, int noNodes, int benefitChosen, int costChosen, double selectionIntensityChosen, double roleConProbChosen, double roleNeighborConProbChosen, double roleMethodCopyProbChosen, double percentCooperators, int delay, string graphQuality, bool isEvolving, double mutationExtremeRMC, double mutationExtremeRMNC)
        {
            InitializeComponent();
            network = new ProsperityNetwork.ProsperitySimulation(noNodes, benefitChosen, costChosen, selectionIntensityChosen, roleConProbChosen, roleNeighborConProbChosen, roleMethodCopyProbChosen, percentCooperators, delay, isEvolving, mutationExtremeRMC, mutationExtremeRMNC);
            evolving = isEvolving;
            paused = false;
            if(graphQuality == "Low")
            {
                quality = Quality.Low;
            }
            else if(graphQuality == "Medium")
            {
                quality = Quality.Medium;
            }
            else if(graphQuality == "Highest")
            {
                quality = Quality.Highest;
            }
            else
            {
                quality = Quality.High;
            }
            prosperityValues = new GearedValues<double>().WithQuality(quality);
            totalCoopValues = new GearedValues<int>().WithQuality(quality);
            avrgCoopsValues = new GearedValues<double>().WithQuality(quality);
            avrgRoleConProbValues = new GearedValues<double>().WithQuality(quality);
            avrgRoleNeighborConProbValues = new GearedValues<double>().WithQuality(quality);
            //SetValue(QualityProperty, graphQuality);
            Task.Factory.StartNew(network.AsyncLoopStart(simName));
            timer = new Timer(GetFromSim, null, delay + 100, delay);
            Trace.WriteLine("--- Timer Start ---");
            //getFromSim();
        }

        /*private void GetFromSim(Object stateInfo)
        {
            if (!paused)
            {
                Dispatcher.Invoke(() =>
                {
                    SetValue(ProsperityProperty, network.Prosperity);
                    Trace.WriteLine("ProsperityProp set called");
                    SetValue(CoopProperty, network.TotalCooperators);
                    Trace.WriteLine("CoopProp set called");
                    *Trace.WriteLine("Prosperity property: " + GetValue(ProsperityProperty));
                    Trace.WriteLine("Coop property: " + GetValue(CoopProperty));
                });
            }
        }*/

        private void GetFromSim(Object stateInfo)
        {
            if (!paused)
            {
                Dispatcher.Invoke(() =>
                {
                    prosperityValues.Add(network.Prosperity);
                    //SetValue(ProsperityProperty, prosperityValues);
                    totalCoopValues.Add(network.TotalCooperators);
                    //SetValue(CoopProperty, totalCoopValues);
                    avrgCoopsValues.Add(network.AverageCooperators);
                    //SetValue(AvrgCoopProperty, prosperityValues);
                    if (evolving)
                    {
                        avrgRoleConProbValues.Add(network.AverageRoleConProb);
                        //SetValue(RoleConProperty, avrgRoleConProbValues);
                        avrgRoleNeighborConProbValues.Add(network.AverageRoleNeighborConProb);
                        //SetValue(RoleNeighborConProperty, avrgRoleNeighborConProbValues);
                    }
                });
            }
        }

        /*private void GetFromSim()
        {
            Action GetFromSim = () =>
            {
                while (!paused)
                {
                    Thread.Sleep(1);
                    SetValue(ProsperityProperty, network.Prosperity);
                    SetValue(CoopProperty, network.TotalCooperators);
                }
            };
            Task.Factory.StartNew(GetFromSim);
        }*/

        private void PauseButton(object sender, RoutedEventArgs e)
        {
            network.ChangeRun();
            paused = !paused;
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e = null)
        {
            PropertyChanged?.Invoke(this, e);
        }

        void WindowClose(object sender, CancelEventArgs e)
        {
            network.StopLoop();
            timer.Dispose();
        }
    }
}
