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
//using System.Runtime.InteropServices;

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
            titleBar = new CustomTitleBar(this, mainGrid, "icon2.ico");//overloaded function that also accepts an icon as parameter, this icon parameter is optional
            //titleBar = new CustomTitleBar(this, mainGrid);
            titleBar.Text = "Comments qgjjjjjjjjjjjjjjjjjjjjjjjjgggggggggggggggggggggggggggggggg";
            titleBar.TextAlignment = HorizontalAlignment.Left;
            titleBar.Height = 26;//recommended height = 26
            titleBar.MinimizeButtonVisibility = Visibility.Collapsed;
            titleBar.IsAutoHide = false;
            titleBar.AutoHideDelay = 2000;
            titleBar.IsPlayAnimation = true;
            titleBar.AnimationInterval = 25;
            titleBar.BackgroundOpacity = 0.5;
            //titleBar.BackgroundColor = Brushes.DarkCyan;
            titleBar.SetBackgroundColorHex("#FF0fa7ff");//00 = black, ff = white
            titleBar.WindowDragMode = CustomTitleBar.DragMode.Both;
            titleBar.DoubleClickResize = true;
            titleBar.FullScreenMode = false;
            titleBar.EnableDrag = true;

            //parent window configurations
            workAreGrid.Margin = new Thickness(0, titleBar.Height, 0,0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(!titleBar.EnableDrag)
                titleBar.EnableDrag = true;
            else
                titleBar.EnableDrag = false;

            btn.Content = titleBar.EnableDrag.ToString();
        }
    }
}
