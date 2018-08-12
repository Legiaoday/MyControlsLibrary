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

namespace TitleBarDebugApp
{
    public partial class MainWindow : Window
    {
        CustomTitleBar titleBar;

        public MainWindow()
        {
            InitializeComponent();
            initTitleBar();
        }

        private void initTitleBar()
        {
            titleBar = new CustomTitleBar(this, mainGrid);
            titleBar.Text = "Comments";
            titleBar.TextAlignment = HorizontalAlignment.Left;
            titleBar.Height = 24;
            titleBar.MinimizeButtonVisibility = Visibility.Collapsed;
            titleBar.IsAutoHide = true;
            titleBar.AutoHideDelay = 2000;
            titleBar.IsPlayAnimation = true;
            titleBar.AnimationInterval = 25;
            titleBar.BackgroundOpacity = 0.5;

            //parent window configurations
            workAreGrid.Margin = new Thickness(0, titleBar.Height, 0,0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(titleBar != null)
            {
                
            }
        }
    }
}
