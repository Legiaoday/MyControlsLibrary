using System.Windows;
using MyControlsLibrary;
using System.Windows.Media;

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
            titleBar.Height = 22;//recommended height = 22
            titleBar.MinimizeButtonVisibility = Visibility.Collapsed;
            titleBar.IsAutoHide = true;
            titleBar.AutoHideDelay = 2000;
            titleBar.IsPlayAnimation = true;
            titleBar.AnimationInterval = 50;
            //titleBar.BackgroundColor = Brushes.DarkCyan;
            titleBar.SetBackgroundColorHex("#FF0fa7ff");//00 = black, ff = white
            titleBar.BackgroundOpacity = 0.7;
            titleBar.WindowDragMode = CustomTitleBar.DragMode.Both;
            titleBar.DoubleClickResize = true;
            titleBar.FullScreenMode = false;
            titleBar.TextColor = Brushes.Black;
            //titleBar.SetTextColorHex("#FF4bff14");//00 = black, ff = white
            titleBar.TBDragStart += new TBDragEventHandler(titleBar_DragStart);
            titleBar.TBDragEnd += new TBDragEventHandler(titleBar_DragEnd);

            //parent window configurations
            workAreGrid.Margin = new Thickness(0, titleBar.Height, 0,0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void titleBar_DragStart(object sender,TBDragEventArgs e)
        {

        }

        private void titleBar_DragEnd(object sender, TBDragEventArgs e)
        {

        }
    }
}
