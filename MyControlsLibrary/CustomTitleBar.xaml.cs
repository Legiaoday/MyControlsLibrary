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
using System.Windows.Threading;
using System.Threading;

namespace MyControlsLibrary
{
    public partial class CustomTitleBar : UserControl
    {
        private Window g_window;

        ///<summary>Creates a new title bar control to be used within another window with a grid as its first child.</summary>
        public CustomTitleBar(Window parentWindow, Grid parentGrid)//both the window and the grid are automatically passed as references
        {
            InitializeComponent();
            parentGrid.Children.Add(this);
            g_window = parentWindow;

            initControls();
        }

        ///<summary>
        ///Creates a new title bar control to be used within another window with a grid as its first child and an icon.
        ///<para>The icon must be a local resource name in the parent window's project (e.g. "icon.ico"). If no icon with the given name is found then the title bar will try to extract the icon from its parent window's assembly.</para>
        ///</summary>
        public CustomTitleBar(Window parentWindow, Grid parentGrid, string icon)//both the window and the grid are automatically passed as references
        {
            InitializeComponent();
            parentGrid.Children.Add(this);
            g_window = parentWindow;

            initControls();
            getIconFromResource(icon);
        }

        #region Generic methods
        private void initControls()
        {
            g_window.WindowStyle = WindowStyle.None;
            g_window.ResizeMode = ResizeMode.NoResize;
            g_window.StateChanged += new EventHandler(g_window__StateChanged);
            g_window.Loaded += new RoutedEventHandler(g_window__Loaded);
        }

        private void changeMaxResButton()
        {
            //changes the character in the maximize/restore button
            if (g_window.WindowState == WindowState.Maximized)
            {
                maximizeButton.Content = 2;
            }
            else if (g_window.WindowState == WindowState.Normal)
            {
                maximizeButton.Content = 1;
            }
        }
        #endregion

        #region Generic events
        private void g_window__StateChanged(object sender, EventArgs e)
        {
            changeMaxResButton();
        }

        private void g_window__Loaded(object sender, RoutedEventArgs e)
        {
            changeMaxResButton();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            g_window.Close();
        }

        private void maximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (g_window.WindowState == WindowState.Maximized)
            {
                g_window.WindowState = WindowState.Normal;
            }
            else if (g_window.WindowState == WindowState.Normal)
            {
                g_window.WindowState = WindowState.Maximized;
            }
        }

        private void minimizeButton_Click(object sender, RoutedEventArgs e)
        {
            g_window.WindowState = WindowState.Minimized;
        }
        #endregion

        #region Move the parent window around
        #region Drag window maximized
        //used in g_window__MouseMove to tell if the drag of the window when in maximized mode should be initiated
        //the window dragging when maximized is being initiated in the g_window__MouseMove event because changing the window state in the dragGrid_MouseDown event will not take effect immediately, thus the offset will be wrong
        bool isDragMaximized = false;
        double dragGridMaxiWidth;//width of the dragGrid when maximized

        //uses rule of three to get the correct offset when the window is maximized
        private void doDragmaximized()
        {
            isDragMaximized = false;
            dragGridMaxiWidth = dragGrid.ActualWidth;//used to get the correct offset when maximized, need to come right before g_window.WindowState = WindowState.Normal;
            g_window.WindowState = WindowState.Normal;

            //at this point the values of dragGrid width and height are NaN or the old values, that's why (g_window.Width - buttonsStackPanel.ActualWidth) is used instead of dragGrid.ActualWidth or dragGrid.Width
            windowDragOffset.X = (g_window.Width - buttonsStackPanel.ActualWidth) * windowDragOffset.X / dragGridMaxiWidth;//rule of three to get the correct offset

            createWindowDragTimer();//used to move the g_window around. A timer is being used instead of the mouse move event because if the user moves the cursor to either the top or left of the window too fast the event won't be able to keep up
            IsDraggingWindow = true;
        }

        private void dragGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && isFirstDrag)
            {
                isFirstDrag = false;

                if (isDragMaximized)
                    doDragmaximized();//used to get the correct offset when maximized
                else if (IsDraggingWindow)
                    createWindowDragTimer();//used to move the g_window around. A timer is being used instead of the mouse move event because if the user moves the cursor to either the top or left of the window too fast the event won't be able to keep up
            }
        }
        #endregion

        private void dragGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                windowDragOffset = e.GetPosition(mainGrid);

                if (g_window.WindowState == WindowState.Maximized)//used to get the correct offset when maximized
                {
                    if (WindowDragMode == DragMode.Both || WindowDragMode == DragMode.Maximized)
                    {
                        isDragMaximized = true;//used to get the correct offset when maximized
                        isFirstDrag = true;
                    }
                }
                if (g_window.WindowState == WindowState.Normal)
                {
                    if (WindowDragMode == DragMode.Both || WindowDragMode == DragMode.Normal)
                    {
                        IsDraggingWindow = true;
                        isFirstDrag = true;
                    } 
                } 
            }
        }

        ///<summary>Indicates if the control is being dragged around by the user.</summary>
        public bool IsDraggingWindow = false;
        private Point windowDragOffset = new Point(0, 0);//used to make the window appear in the right location when the user drags the title bar
        private DispatcherTimer windowDragTimer;//used to move the window around. A timer is being used instead of the mouse move event because if the user moves the cursor to either the top or left of the window too fast the event won't be able to keep up
        private bool isFirstDrag = false;//used in the dragGrid_MouseMove event to initiate the dragging of the window only once (because the mouse move keeps being fired as long as the user moves the mouse cursor)

        private void createWindowDragTimer()
        {
            windowDragTimer = new DispatcherTimer();
            windowDragTimer.Interval = TimeSpan.FromMilliseconds(1);
            windowDragTimer.Tick += new EventHandler(moveWindow_Tick);
            windowDragTimer.Start();
        }

        void moveWindow_Tick(object sender, EventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)//this is here to avoid g_window.DragMove() throwing an error
            {
                doSetWindowPos();
                g_window.DragMove();
            }
            else//this else exists because there's a bug where the dragging stop but g_window_MouseUp is not called. This usually happens when the user moves the cursor too fast to the right while releasing the dragging
            {
                endTempTabWinDrag();
            }
        }

        private void doSetWindowPos()
        {
            g_window.Left = System.Windows.Forms.Cursor.Position.X - windowDragOffset.X;
            g_window.Top = System.Windows.Forms.Cursor.Position.Y - windowDragOffset.Y;
        }

        private void endTempTabWinDrag()
        {
            windowDragTimer.Stop();
            IsDraggingWindow = false;
        }

        #endregion

        #region Hide/show controls
        #region Auto hide animation
        private bool isPlayAnimation = true;//sets if the the animation will be used to hide/show the control
        ///<summary>Sets if the control will have an animation when hiding/showing the control.</summary>
        public bool IsPlayAnimation
        {
            get { return isPlayAnimation; }

            set
            {
                isPlayAnimation = value;
                if (!value) cancelAnimation();
            }
        }
        ///<summary>Sets how fast the animation plays in milliseconds. Lower values = fast animation, higher values = slow animation. Default value = 25.</summary>
        public int AnimationInterval = 25;
        private bool isPlayingHideAnim = false;//indicates if the hide animation is being played
        private bool isPlayingShowAnim = false;//indicates if the show animation is being played
        private DispatcherTimer controlAnimTimer;//main timer that will update the animation of the control based on isPlayingHideAnim and isPlayingShowAnim

        private void startHideAnimation()
        {
            if (!isPlayingHideAnim && !isPlayingShowAnim)
            {
                mainGrid.VerticalAlignment = VerticalAlignment.Top;
                mainGrid.Height = this.ActualHeight;//used to avoid a bug that resizes the mainGrid's height to 25.96 when the hide/show animation plays
                controlAnimTimer = new DispatcherTimer();
                controlAnimTimer.Interval = TimeSpan.FromMilliseconds(AnimationInterval);
                controlAnimTimer.Tick += new EventHandler(doAnimation_Tick);
                controlAnimTimer.Start();
            }

            isPlayingHideAnim = true;
            isPlayingShowAnim = false;
        }

        private void startShowAnimation()
        {
            if (!isPlayingHideAnim && !isPlayingShowAnim)
            {
                mainGrid.VerticalAlignment = VerticalAlignment.Top;
                mainGrid.Height = this.ActualHeight;//used to avoid a bug that resizes the mainGrid's height to 25.96 when the hide/show animation plays
                controlAnimTimer = new DispatcherTimer();
                controlAnimTimer.Interval = TimeSpan.FromMilliseconds(AnimationInterval);
                controlAnimTimer.Tick += new EventHandler(doAnimation_Tick);
                controlAnimTimer.Start();
            }

            isPlayingShowAnim = true;
            isPlayingHideAnim = false;
        }

        private void doAnimation_Tick(object sender, EventArgs e)
        {
            if (isPlayingHideAnim)//it's hiding
            {
                if (mainGrid.Margin.Top >= (this.ActualHeight * -1) - 1)//(this.ActualHeight * -1) -1) inverts the value of this.ActualHeigh and then subtracts one from it, this last subtraction is so that one pixel of the maiGrid doesn't show up while hidden
                {
                    mainGrid.Margin = new Thickness(0, mainGrid.Margin.Top - 1, 0, 0);//makes the grid slides to the top
                }
                else
                {
                    controlAnimTimer.Stop();
                    isPlayingHideAnim = false;
                }
            }
            else if (isPlayingShowAnim)//it's showing
            {
                if (mainGrid.Margin.Top < 0)
                {
                    mainGrid.Margin = new Thickness(0, mainGrid.Margin.Top + 1, 0, 0);//makes the grid slides down from the top
                }
                else
                {
                    controlAnimTimer.Stop();
                    isPlayingShowAnim = false;
                }
            }
        }

        private void cancelAnimation()
        {
            if (isPlayingHideAnim || isPlayingShowAnim) controlAnimTimer.Stop();

            isPlayingHideAnim = false;
            isPlayingShowAnim = false;
            mainGrid.Margin = new Thickness(0, 0, 0, 0);
            mainGrid.VerticalAlignment = VerticalAlignment.Stretch;
        }
        #endregion

        private bool isAutoHide = false;//private backing flag for auto hiding
        ///<summary>Sets if the control will auto hide after a certain period of time.</summary>
        public bool IsAutoHide
        {
            get { return isAutoHide; }

            set
            {
                isAutoHide = value;

                if (!value) showControl(); else hideControl();
            }
        }
        ///<summary>Time in milliseconds that will pass before the control auto hides.</summary>
        public int AutoHideDelay = 2000;
        private CancellationTokenSource cancelTokenTitleBar = new CancellationTokenSource();

        private async void hideControl()
        {
            try
            {
                await Task.Delay(AutoHideDelay, cancelTokenTitleBar.Token);

                if (!isPlayAnimation)
                    mainGrid.Visibility = Visibility.Hidden;
                else if (!IsDraggingWindow)//this is here because of the dragging window when maximized
                    startHideAnimation();
            }
            catch (TaskCanceledException ex)
            {

            }
        }

        private void showControl()
        {
            canceLTokenTitleBar();

            if (!isPlayAnimation)
            {
                mainGrid.Visibility = Visibility.Visible;
            }
            else
            {
                startShowAnimation();
            }
        }

        private void canceLTokenTitleBar()
        {
            cancelTokenTitleBar.Cancel();
            cancelTokenTitleBar = new CancellationTokenSource();
        }

        private void windowHeader_MouseEnter(object sender, MouseEventArgs e)
        {
            if (isAutoHide)
            {
                showControl();
            }
        }

        private void windowHeader_MouseLeave(object sender, MouseEventArgs e)
        {
            if (isAutoHide)
            {
                hideControl();
            }
        }
        #endregion

        #region Adjust controls' margins
        private Thickness resizeThic;

        private void buttonsStackPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            resizeControls();
        }

        private void resizeControls()//adjust the right margin of headerLabel and dragGrid based on the width of buttonsStackPanel (a dynamically sizable stack panel)
        {
            resizeThic = dragGrid.Margin;
            resizeThic.Right = buttonsStackPanel.ActualWidth;
            dragGrid.Margin = resizeThic;

            resizeThic = headerLabel.Margin;
            resizeThic.Right = buttonsStackPanel.ActualWidth;
            headerLabel.Margin = resizeThic;
        }
        #endregion

        #region Generic public properties
        #region Background transparency
        private double backgroundOpacity = 1;
        ///<summary>
        ///Transparency value of the background. Values between 0 and 1, where 0 = completely transparent and 1 = completely opaque. For it to work the parent window AllowsTransparency property must be set to true.
        ///<para>The AllowsTransparency property of a WPF window can only be set if the window has not been shown yet.</para>
        ///</summary>
        public double BackgroundOpacity
        {
            get { return backgroundOpacity; }

            set
            {
                backgroundOpacity = value;
                applyBackgroundTransparency();
            }
        }

        private void applyBackgroundTransparency()
        {
            if (g_window.AllowsTransparency)//AllowsTransparency cannot be set after the window is shown, that's why it needs to be set in the xaml of the parent window or before the InitializeComponent() method in the parent window
            {
                g_window.WindowStyle = WindowStyle.None;//for the AllowsTransparency to work the window style needs to be set to none
                this.Background = Brushes.Transparent;
                headerLabel.Background = Brushes.Transparent;
                dragGrid.Background = Brushes.Transparent;
                buttonsStackPanel.Background = Brushes.Transparent;
                mainGrid.Background.Opacity = BackgroundOpacity;
            }
            else
            {
                if(backgroundOpacity < 1)
                {
                    MessageBox.Show("Background transparency can only be enabled if the window's AllowsTransparency property is set to true.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        #endregion

        #region Background color
        ///<summary>Background color of the title bar.</summary>
        public Brush BackgroundColor
        {
            get { return mainGrid.Background; }

            set { mainGrid.Background = value; }
        }

        /// <summary>
        /// Sets the background color based on a hexadecimal color code. The color code is composed of four sets of two hexadecimal numbers like for example: #FF0fa7ff
        /// <para>The first two digits being the alpha transparency (FF), the third and fourth being the red (0f), the fifth and sixty being the green (a7) and the seventh and eighth being the blue (ff). You can omit the transparency if you wish.</para>
        /// </summary>
        public void SetBackgroundColorHex(string hexColor)
        {
            BrushConverter converter = new BrushConverter();
            Brush brush = (Brush)converter.ConvertFromString(hexColor);
            mainGrid.Background = brush;
        }
        #endregion

        ///<summary>Text to be displayed in the title bar.</summary>
        public string Text
        {
            get { return headerLabel.Content.ToString(); }
            set { headerLabel.Content = value; }
        }

        ///<summary>Title bar's text alignment.</summary>
        public HorizontalAlignment TextAlignment
        {
            get { return headerLabel.HorizontalContentAlignment; }
            set { headerLabel.HorizontalContentAlignment = value; }
        }

        ///<summary>Changes the visibility of the minimize button.</summary>
        public Visibility MinimizeButtonVisibility
        {
            get { return minimizeButton.Visibility; }
            set { minimizeButton.Visibility = value; }
        }

        ///<summary>Changes the visibility of the maximize button.</summary>
        public Visibility MaximizeButtonVisibility
        {
            get { return maximizeButton.Visibility; }
            set { maximizeButton.Visibility = value;}
        }

        ///<summary>Changes the visibility of the close button.</summary>
        public Visibility CloseButtonVisibility
        {
            get { return closeButton.Visibility; }
            set { closeButton.Visibility = value; }
        }

        public DragMode WindowDragMode = DragMode.Both; 
        public enum DragMode
        {
            ///<summary>The window cannot be dragged at all.</summary>
            None,
            ///<summary>The window can only be dragged when its state is set to normal.</summary>
            Normal,
            ///<summary>The window can only initiate drag when its state is set to maximized.</summary>
            Maximized,
            ///<summary>The window can be dragged when in both normal and maximized states.</summary>
            Both
        }
        #endregion

        #region Title bar icon
        private void getIconFromResource(string iconResourceOrPath)
        {
            try
            {
                BitmapImage icon = new BitmapImage();
                icon.BeginInit();
                icon.UriSource = new Uri("pack://application:,,,/" + iconResourceOrPath);
                icon.EndInit();

                titleBarImgIcon.Source = icon;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
                getIconFromAssembly();
            }
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll", SetLastError = true)]
        private static extern bool DeleteObject(IntPtr hObject);

        private void getIconFromAssembly()
        {
            try
            {
                System.Reflection.Assembly currentAssembly = System.Reflection.Assembly.GetEntryAssembly();//gets the title bar parent window's assembly path
                System.Drawing.Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(currentAssembly.Location);
                System.Drawing.Bitmap bitmap = icon.ToBitmap();
                icon.Dispose();
                IntPtr hBitmap = bitmap.GetHbitmap();

                ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                titleBarImgIcon.Source = wpfBitmap;

                if (!DeleteObject(hBitmap)) throw new System.ComponentModel.Win32Exception();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        //fix the dragGrid cursor when drag is disabled
        //add double click maximize title bar
    }
}