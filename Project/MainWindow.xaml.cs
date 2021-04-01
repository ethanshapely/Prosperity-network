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

        private void GenerateSim(object sender, RoutedEventArgs e)
        {
            SimWindow sim = new SimWindow();
            sim.Show();
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

        private void UpArrow(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Grid grid = button.Parent as Grid;
            TextBox textBox = grid.FindName(button.Tag.ToString()) as TextBox;
            try
            {
                textBox.Text = (int.Parse(textBox.Text) + 1).ToString();
            }
            catch (FormatException err)
            {
                textBox.Text = "50";
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
            }
            catch (FormatException err)
            {
                textBox.Text = "50";
            }
        }
    }
}
