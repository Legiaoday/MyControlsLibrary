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

namespace MyControlsLibrary
{
    public partial class NumericUpDown : UserControl
    {
        public NumericUpDown()
        {
            InitializeComponent();
        }

        private void NumberTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void NumberTxt_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
                e.Handled = true;
        }

        private void NumberTxt_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void upButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int temp = Int32.Parse(numberTxt.Text);

                temp++;
                numberTxt.Text = temp.ToString();
            }
            catch (FormatException ex)
            {
                numberTxt.Text = "1";
            }
        }

        private void downButton_Click(object sender, RoutedEventArgs e) 
        {
            try
            {
                int temp = Int32.Parse(numberTxt.Text);

                if (temp > 0)
                {
                    temp--;
                    numberTxt.Text = temp.ToString();
                }
            }
            catch (FormatException ex)
            {
                numberTxt.Text = "0";
            }
        }
    }
}
