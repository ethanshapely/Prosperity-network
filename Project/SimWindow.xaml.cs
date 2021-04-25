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

namespace Project
{
    /// <summary>
    /// Interaction logic for SimWindow.xaml
    /// </summary>
    public partial class SimWindow : Window
    {
        ///private int networkCooperators;
        ///private double networkProsperity;
        ProsperityNetwork.ProsperitySimulation network;

        public static readonly DependencyProperty QualityProperty = DependencyProperty.Register("GraphQuality", typeof(string), typeof(SimWindow), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty ProsperityProperty = DependencyProperty.Register("NetworkProsperity", typeof(double), typeof(SimWindow), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty CoopProperty = DependencyProperty.Register("NetworkNoCoops", typeof(int), typeof(SimWindow), new FrameworkPropertyMetadata(null));
        private Boolean paused;
        private Timer timer;

        private string GraphQuality
        {
            get { return (string)GetValue(QualityProperty); }
        }

        private double NetworkProsperity
        {
            get { return (double)GetValue(ProsperityProperty); }
        }

        private int NetworkNoCoops
        {
            get { return (int)GetValue(CoopProperty); }
        }

        public SimWindow(string simName, int noNodes, int benefitChosen, int costChosen, double selectionIntensityChosen, double roleConProbChosen, double roleNeighborConProbChosen, double roleMethodCopyProbChosen, double percentCooperators, int delay, string graphQuality, bool isEvolving, double mutationExtreme)
        {
            InitializeComponent();
            network = new ProsperityNetwork.ProsperitySimulation(noNodes, benefitChosen, costChosen, selectionIntensityChosen, roleConProbChosen, roleNeighborConProbChosen, roleMethodCopyProbChosen, percentCooperators, delay, isEvolving, mutationExtreme);
            network.AsyncLoopStart(simName);
            paused = false;
            SetValue(QualityProperty, graphQuality);
            timer = new Timer(GetFromSim, null, (delay*1000), (delay * 1000));
            //getFromSim();
        }

        private void GetFromSim(Object stateInfo)
        {
            if (!paused)
            {
                Dispatcher.Invoke(() =>
                {
                    SetValue(ProsperityProperty, network.Prosperity);
                    Trace.WriteLine("ProsperityProp set called");
                    SetValue(CoopProperty, network.TotalCooperators);
                    Trace.WriteLine("CoopProp set called");
                });
            }
        }

        /*private void getFromSim()
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

        /*public SimWindow(int noNodes, int benefitChosen, int costChosen, double selectionIntensityChosen, double roleConProbChosen, double roleNeighborConProbChosen, double roleMethodCopyProbChosen, double percentCooperators) : this()
        {
            ProsperityNetwork.Network network = new ProsperityNetwork.Network(noNodes, benefitChosen, costChosen, selectionIntensityChosen, roleConProbChosen, roleNeighborConProbChosen, roleMethodCopyProbChosen, percentCooperators);
            /// Main loop: 
            /// - Async loop
            /// - Loop lasts until window is closed or stop is pressed
            /// - Loop pauses when pause pressed (nested if in while using 'run' variable(?))
            /// - Loop passes Variables to Graph controlers
        }*/

        void WindowClose(object sender, CancelEventArgs e)
        {
            network.StopLoop();
        }
    }
}
