using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Project
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Returns true if TextBox contents are numeric
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]").IsMatch(e.Text);
        }

        // Add validation for each data entry that requires one i.e. validate simulation name to work in files, convert percentages to decimal, etc.

        private void UpArrow(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Grid grid = button.Parent as Grid;
            TextBox textBox = grid.FindName(button.Tag.ToString()) as TextBox;
            try
            {
                if(!(textBox.Tag.ToString() == "Percent" && int.Parse(textBox.Text) == 99))
                {
                    textBox.Text = (int.Parse(textBox.Text) + 1).ToString();
                }
            }
            catch (FormatException err)
            {
                textBox.Text = "50";
                Trace.WriteLine(err);
            }
        }

        private void DownArrow(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Grid grid = button.Parent as Grid;
            TextBox textBox = grid.FindName(button.Tag.ToString()) as TextBox;
            try
            {
                textBox.Text = (int.Parse(textBox.Text) - 1).ToString();
                if(textBox.Tag.ToString() == "Percent" && textBox.Text == "0")
                {
                    textBox.Text = "1";
                }
                else if(int.Parse(textBox.Text) < 0)
                {
                    textBox.Text = "0";
                }
            }
            catch (FormatException err)
            {
                textBox.Text = "50";
                Trace.WriteLine(err);
            }
        }

        private void GenerateSim(object sender, RoutedEventArgs e)
        {
            try
            {
                Trace.WriteLine("Percentage Cooperators(int): " + int.Parse(percCooperators.Text));
                Trace.WriteLine("Percentage Cooperators(double): " + ((double)int.Parse(percCooperators.Text) / 100));
                SimWindow sim = new SimWindow(
                    simName.Text,
                    int.Parse(noNodes.Text),
                    int.Parse(benefit.Text),
                    int.Parse(cost.Text),
                    (double)int.Parse(selectionIntensity.Text),
                    (double)int.Parse(roleModelConProb.Text) / 100,
                    (double)int.Parse(roleModelNeighborConProb.Text) / 100,
                    (double)int.Parse(roleModelCopyProb.Text) / 100,
                    (double)int.Parse(percCooperators.Text) / 100,
                    int.Parse(updateDelay.Text),
                    qualityBox.SelectedValue.ToString(),
                    (bool)evolveCheck.IsChecked,
                    (double)int.Parse(mutationExtreme.Text) / 100
                );
                sim.Show();
            }
            catch(FormatException err)
            {
                Console.WriteLine("A formatting error occurred: " + err);
            }
        }
    }
}
