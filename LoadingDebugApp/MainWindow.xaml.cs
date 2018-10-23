using System.Windows;
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
            loading.AnimationSpeed = 100;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!loading.IsPlayingAnimation)
            {
                loading.StartAnimation();
            }
            else
            {
                loading.StopAnimation(1000);
            }
        }
    }
}
