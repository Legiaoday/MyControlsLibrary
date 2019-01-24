using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
            if (!AllowNegative) minValue = 0;
            numberTxt.IsReadOnly = true;
        }

        #region Generic events
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
        #endregion

        #region Generic public properties
        /// <summary>Sets whether or not show error messages when Value is out of range. Default value is false.</summary>
        public bool ShowValueOutOfRangeErrors { get; set; } = false;

        /// <summary>Sets whether or not the numeric up/down control should allow negative numbers. Default value is false.</summary>
        public bool AllowNegative { get; set; } = false;

        /// <summary>Sets whether or not the text box is read only. Default value is true.</summary>
        public bool IsTextBoxReadOnly
        {
            get { return numberTxt.IsReadOnly; }
            set { numberTxt.IsReadOnly = value; }
        }

        /// <summary>Current value of the numeric up/down text box.</summary>
        public int Value
        {
            get
            {
                try
                {
                    return Int32.Parse(numberTxt.Text);
                }
                catch(FormatException ex)
                {
                    MessageBox.Show("Error parsing numberTxt.Text.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return minValue;
                }
                catch (Exception)
                {
                    throw;
                }
            }
            set
            {
                if (value >= minValue && value <= maxValue)
                    if (value >= 0 || AllowNegative)
                        numberTxt.Text = value.ToString();
                    else
                        if (ShowValueOutOfRangeErrors) MessageBox.Show("Value cannot be negative. Negative numbers are not allowed unless AllowNegative is set to true.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                   if (ShowValueOutOfRangeErrors) MessageBox.Show("Value is out of range. Value must be great or equal to MinValue and lesser or equal to MaxValue.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int maxValue = 2147483647;
        /// <summary>Maximum value the numeric text box can hold. Default value is 2,147,483,647.</summary>
        public int MaxValue
        {
            get { return maxValue; }
            set
            {
                if (value > minValue)
                    if (value >= 0 || AllowNegative)//checks if negative numbers are allowed
                        maxValue = value;
                    else
                        MessageBox.Show("MaxValue cannot be negative. Negative numbers are not allowed.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("MaxValue cannot be lower or equal to MinValue.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private int minValue = -2147483648;
        /// <summary>Minimum value the numeric text box can hold. Default value is -2,147,483,648 or 0 if AllowNegative is false.</summary>
        public int MinValue
        {
            get { return minValue; }
            set
            {
                if (value < maxValue)
                    if (value >= 0 || AllowNegative)//checks if negative numbers are allowed
                        minValue = value;
                    else
                        MessageBox.Show("MinValue cannot be begative. Negative numbers are not allowed.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("MinValue cannot be greater or equal to MaxValue.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        #region Increment/decrement number
        private CancellationTokenSource cancelTokenSrc = new CancellationTokenSource();
        private bool isHoldingDownButton = false;

        private void upButton_Click(object sender, RoutedEventArgs e)
        {
            if (!HoldDownToIncrease) ++Value;
        }

        private void downButton_Click(object sender, RoutedEventArgs e)
        {
            if (!HoldDownToIncrease) --Value;
        }

        private async void increaseNumber()
        {
            isHoldingDownButton = true;

            while (isHoldingDownButton)
            {
                ++Value;//this must come before the await otherwise the number won't increase if the mouse/up event action were lesser than 1000/holdDownSpeed in milliseconds
                try
                {
                    await Task.Delay(1000/holdDownSpeed, cancelTokenSrc.Token);
                }
                catch (TaskCanceledException ex)
                {
                }
            }
        }

        private void UpButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (HoldDownToIncrease) increaseNumber();
        }

        private void UpButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (HoldDownToIncrease) cancelToken();
        }

        private async void decreaseNumber()
        {
            isHoldingDownButton = true;

            while (isHoldingDownButton)
            {
                --Value;//this must come before the await otherwise the number won't decrease if the mouse/up event action were lesser than 1000/holdDownSpeed in milliseconds
                try
                {
                    await Task.Delay(1000/holdDownSpeed, cancelTokenSrc.Token);
                }
                catch (TaskCanceledException ex)
                {
                }
            }
        }

        private void DownButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (HoldDownToIncrease) decreaseNumber();
        }

        private void DownButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (HoldDownToIncrease) cancelToken();
        }

        public void cancelToken()
        {
            isHoldingDownButton = false;
            cancelTokenSrc.Cancel();
            cancelTokenSrc = new CancellationTokenSource();
        }
        #endregion

        private byte gradualSpeedModifier = 0;//used gradually change in real time the speed of the increment/decrement of the number when holding down the button

        private byte gradualSpeedIndex = 1;//values between 1 and 10. 1 being the default value

        /// <summary>Sets whether or not holding down the button will increase/decrease the number rapidly. Default value is true.</summary>
        public bool HoldDownToIncrease { get; set; } = true;

        private byte holdDownSpeed = 10;
        /// <summary>The speed which the number will increase/decrease when the user holds down the button (only works if HoldDownToIncrease is set to true). Can only accept values between 1 and 100. Default value is 10.</summary>
        public byte HoldDownSpeed
        {
            get { return holdDownSpeed; }
            set
            {
                if (value > 0 && value <= 100)
                    holdDownSpeed = value;
                else
                    MessageBox.Show("Value out of range. Only values between 1 and 100 can be assigned to HoldDownSpeed.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
