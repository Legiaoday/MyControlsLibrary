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
            loading.CLAnimationStarted += new CLAnimationEventHandler(loadAnim_Start);
            loading.CLAnimationStopped += new CLAnimationEventHandler(loadAnim_Stop);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!loading.IsPlayingAnimation)
            {
                loading.StartAnimation();
                //loading.StartAnimation(2000);
            }
            else
            {
                loading.StopAnimation(1000);
                //loading.StopAnimation();
            }
        }

        private void loadAnim_Start(object sender, CLAnimationEventArgs e)
        {

        }

        private void loadAnim_Stop(object sender, CLAnimationEventArgs e)
        {

        }
    }
}
