using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;

namespace MyControlsLibrary
{
    public partial class CustomLoading : UserControl
    {
        private Rectangle square1;
        private Rectangle square2;
        private Rectangle square3;
        private Rectangle square4;
        private Rectangle square5;
        private Thread loadingThread;
        private DispatcherTimer timer;
        DispatcherTimer tempTimer;
        private double opacitySub1 = 20;
        private double opacitySub2 = 40;
        private double opacitySub3 = 60;
        private double opacitySub4 = 80;
        private double opacitySub5 = 100;
        ///<summary>Indicates whether the animation is playing or not.</summary>
        public bool IsPlayingAnimation { get; private set; } = false;//read only for anyone outside of this class
        private double animationSpeed = 100;
        ///<summary>Speed which the animation plays. Default value = 100ms.</summary>
        public double AnimationSpeed
        {
            get { return animationSpeed; }

            set
            {
                animationSpeed = value;
                timer.Interval = TimeSpan.FromMilliseconds(animationSpeed);
            }
        }

        ///<summary>Creates a new loading control.</summary>
        public CustomLoading()
        {
            InitializeComponent();
            initObjects();
        }

        private void initObjects()
        {
            this.Background = new SolidColorBrush() { Color = (Color)ColorConverter.ConvertFromString("#FF171717"), Opacity = 1 };
            this.Width = 128;
            this.Height = 32;
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.VerticalAlignment = VerticalAlignment.Center;
            this.IsHitTestVisible = false;
            this.Visibility = Visibility.Collapsed;

            square1 = new Rectangle();
            square1.Width = 16;
            square1.Height = 16;
            square1.Fill = new SolidColorBrush() { Color = Colors.AliceBlue, Opacity = 0.2 };
            square1.HorizontalAlignment = HorizontalAlignment.Left;
            square1.VerticalAlignment = VerticalAlignment.Top;
            square1.Margin = new Thickness(8, 8, 0, 0);

            square2 = new Rectangle();
            square2.Width = 16;
            square2.Height = 16;
            square2.Fill = new SolidColorBrush() { Color = Colors.AliceBlue, Opacity = 0.4 };
            square2.HorizontalAlignment = HorizontalAlignment.Left;
            square2.VerticalAlignment = VerticalAlignment.Top;
            square2.Margin = new Thickness(32, 8, 0, 0);

            square3 = new Rectangle();
            square3.Width = 16;
            square3.Height = 16;
            square3.Fill = new SolidColorBrush() { Color = Colors.AliceBlue, Opacity = 0.6 };
            square3.HorizontalAlignment = HorizontalAlignment.Left;
            square3.VerticalAlignment = VerticalAlignment.Top;
            square3.Margin = new Thickness(56, 8, 0, 0);

            square4 = new Rectangle();
            square4.Width = 16;
            square4.Height = 16;
            square4.Fill = new SolidColorBrush() { Color = Colors.AliceBlue, Opacity = 0.8 };
            square4.HorizontalAlignment = HorizontalAlignment.Left;
            square4.VerticalAlignment = VerticalAlignment.Top;
            square4.Margin = new Thickness(80, 8, 0, 0);

            square5 = new Rectangle();
            square5.Width = 16;
            square5.Height = 16;
            square5.Fill = new SolidColorBrush() { Color = Colors.AliceBlue, Opacity = 1 };
            square5.HorizontalAlignment = HorizontalAlignment.Left;
            square5.VerticalAlignment = VerticalAlignment.Top;
            square5.Margin = new Thickness(104, 8, 0, 0);

            mainGrid.Children.Add(square1);
            mainGrid.Children.Add(square2);
            mainGrid.Children.Add(square3);
            mainGrid.Children.Add(square4);
            mainGrid.Children.Add(square5);

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(animationSpeed);
            timer.Tick += new EventHandler(timer_Tick);
        }

        ///<summary>Starts playing the animation for an indefinite amount of time.</summary>
        public void StartAnimation()
        {
            //To make sure we only trigger the event if a handler is present
            //we check the event to make sure it's not null.
            CLAnimationStarted?.Invoke(this, new CLAnimationEventArgs());

            loadingThread = new Thread(loadingThreadWork);
            loadingThread.IsBackground = true;
            loadingThread.Start();
        }

        ///<summary>Starts playing the animation and then stops after a certain amount of time in milliseconds.</summary>
        public void StartAnimation(double timeSpan)
        {
            //To make sure we only trigger the event if a handler is present
            //we check the event to make sure it's not null.
            CLAnimationStarted?.Invoke(this, new CLAnimationEventArgs());

            loadingThread = new Thread(loadingThreadWork);
            loadingThread.IsBackground = true;
            loadingThread.Start();

            tempTimer = new DispatcherTimer();
            tempTimer.Interval = TimeSpan.FromMilliseconds(timeSpan);
            tempTimer.Tick += new EventHandler(stopTimer_Tick);
            tempTimer.Start();
        }

        private void loadingThreadWork()
        {
            Application.Current.Dispatcher.Invoke(() => this.Visibility = Visibility.Visible);
            IsPlayingAnimation = true;
            timer.Start();
        }

        ///<summary>Stops playing the animation.</summary>
        public void StopAnimation()
        {
            IsPlayingAnimation = false;
            this.Visibility = Visibility.Collapsed;
            timer.Stop();
            square1.Fill = new SolidColorBrush() { Color = Colors.AliceBlue, Opacity = 0.2 };
            square2.Fill = new SolidColorBrush() { Color = Colors.AliceBlue, Opacity = 0.4 };
            square3.Fill = new SolidColorBrush() { Color = Colors.AliceBlue, Opacity = 0.6 };
            square4.Fill = new SolidColorBrush() { Color = Colors.AliceBlue, Opacity = 0.8 };
            square5.Fill = new SolidColorBrush() { Color = Colors.AliceBlue, Opacity = 1 };
            opacitySub1 = 20;
            opacitySub2 = 40;
            opacitySub3 = 60;
            opacitySub4 = 80;
            opacitySub5 = 100;

            //To make sure we only trigger the event if a handler is present
            //we check the event to make sure it's not null.
            CLAnimationStopped?.Invoke(this, new CLAnimationEventArgs());
        }

        ///<summary>Stops playing the animation after a certain amount of time in milliseconds.</summary>
        public void StopAnimation(double timeSpan)
        {
            tempTimer = new DispatcherTimer();
            tempTimer.Interval = TimeSpan.FromMilliseconds(timeSpan);
            tempTimer.Tick += new EventHandler(stopTimer_Tick);
            tempTimer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            square1.Fill = new SolidColorBrush() { Color = Colors.AliceBlue, Opacity = opacitySub1 / 100 };
            square2.Fill = new SolidColorBrush() { Color = Colors.AliceBlue, Opacity = opacitySub2 / 100 };
            square3.Fill = new SolidColorBrush() { Color = Colors.AliceBlue, Opacity = opacitySub3 / 100 };
            square4.Fill = new SolidColorBrush() { Color = Colors.AliceBlue, Opacity = opacitySub4 / 100 };
            square5.Fill = new SolidColorBrush() { Color = Colors.AliceBlue, Opacity = opacitySub5 / 100 };

            opacitySub1 = (opacitySub1 > 10) ? opacitySub1 -= 10 : opacitySub1 = 100;
            opacitySub2 = (opacitySub2 > 10) ? opacitySub2 -= 10 : opacitySub2 = 100;
            opacitySub3 = (opacitySub3 > 10) ? opacitySub3 -= 10 : opacitySub3 = 100;
            opacitySub4 = (opacitySub4 > 10) ? opacitySub4 -= 10 : opacitySub4 = 100;
            opacitySub5 = (opacitySub5 > 10) ? opacitySub5 -= 10 : opacitySub5 = 100;
        }

        private void stopTimer_Tick(object sender, EventArgs e)
        {
            tempTimer.Stop();
            StopAnimation();

            //To make sure we only trigger the event if a handler is present
            //we check the event to make sure it's not null.
            CLAnimationStopped?.Invoke(this, new CLAnimationEventArgs());
        }

        #region Animation started/ended events
        ///<summary>Occurs when the animation starts.</summary>
        public event CLAnimationEventHandler CLAnimationStarted;//event name that will be called by the programs that use this library
        ///<summary>Occurs when the animation stops.</summary>
        public event CLAnimationEventHandler CLAnimationStopped;//event name that will be called by the programs that use this library
        #endregion
    }

    #region Animation started/ended event class, handle and delegate
    //these need to be out of the escope of the main class
    public delegate void CLAnimationEventHandler(object source, CLAnimationEventArgs e);

    public class CLAnimationEventArgs : EventArgs
    {
        public CLAnimationEventArgs()
        {
        }
    } 
    #endregion
}
