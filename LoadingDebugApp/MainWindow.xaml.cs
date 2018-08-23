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
using MyControlsLibrary;

namespace LoadingDebugApp
{
    public partial class MainWindow : Window
    {
        CustomLoading loading;

        public MainWindow()
        {
            InitializeComponent();
            loading = new CustomLoading();
            mainGrid.Children.Add(loading);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(!loading.IsPlayingAnimation)
            {
                loading.StartAnimation();
            }
            else
            {
                loading.StopAnimation();
            }
        }
    }
}
