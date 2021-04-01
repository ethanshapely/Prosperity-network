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

namespace Project
{
    /// <summary>
    /// Interaction logic for SimWindow.xaml
    /// </summary>
    public partial class SimWindow : Window
    {
        ///private int noNodes, benefitChosen, costChosen;
        ///private double selectionIntensityChosen, roleConProbChosen, roleNeighborConProbChosen, roleMethodCopyProbChosen, percentCooperators;
        private Boolean loop, run;

        public SimWindow()
        {
            InitializeComponent();
        }

        public SimWindow(int noNodes, int benefitChosen, int costChosen, double selectionIntensityChosen, double roleConProbChosen, double roleNeighborConProbChosen, double roleMethodCopyProbChosen, double percentCooperators) : this()
        {
            ProsperityNetwork.Network network = new ProsperityNetwork.Network(noNodes, benefitChosen, costChosen, selectionIntensityChosen, roleConProbChosen, roleNeighborConProbChosen, roleMethodCopyProbChosen, percentCooperators);
            /// Main loop: 
            /// - Async loop
            /// - Loop lasts until window is closed or stop is pressed
            /// - Loop pauses when pause pressed (nested if in while using 'run' variable(?))
            /// - Loop passes Variables to Graph controlers
        }
    }
}
