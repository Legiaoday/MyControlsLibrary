﻿using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MyControlsLibrary
{
    public partial class NumericUpDown : UserControl
    {
        public NumericUpDown()
        {
            InitializeComponent();
            numberTxt.IsReadOnly = true;
            AllowNegative = true;//needed to trigger AllowNegative's set and setup the minValue variable and numberTxt.Text
        }

        #region Generic events
        private void NumberTxt_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
                e.Handled = true;
        }

        private void NumberTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                if (e.Text == "-" && AllowNegative)
                {
                    if (numberTxt.Text.Length == 0 || numberTxt.SelectionLength == 0 && numberTxt.CaretIndex == 0 && char.IsDigit(numberTxt.Text, 0))
                    {
                        numberTxt.Text = "-" + numberTxt.Text;
                    }
                }

                e.Handled = true;
            }
            else if (numberTxt.Text.Length > 0 && numberTxt.Text[0] == '-' && numberTxt.CaretIndex == 0)
            {
                e.Handled = true;
            }    
        }

        private void NumberTxt_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBoxPreviewKeyDown?.Invoke(sender, e);//checks if the handler is not null and then triggers the event. The handler is only valid when the class using this library assigns the public event PreviewKeyDown to an event of that class

            //if (e.Key == Key.Back)
            //{
            //    if (numberTxt.Text.Length == 2 && numberTxt.Text[0] == '-' && numberTxt.CaretIndex == 2)
            //    {
            //        numberTxt.Text = "";
            //        e.Handled = true;
            //    }
            //}
            //else if (e.Key == Key.Delete)
            //{
            //    if (numberTxt.Text.Length == 2 && numberTxt.Text[0] == '-' && numberTxt.CaretIndex == 1)
            //    {
            //        numberTxt.Text = "";
            //        e.Handled = true;
            //    }
            //}
            /*else*/
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void NumberTxt_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBoxPreviewKeyUp?.Invoke(sender, e);//checks if the handler is not null and then triggers the event. The handler is only valid when the class using this library assigns the public event PreviewKeyDown to an event of that class
        }
        #endregion

        #region Public events
        /// <summary>Occurs when the user presses down a key on the text box.</summary>
        public event KeyEventHandler TextBoxPreviewKeyDown;

        /// <summary>Occurs when the user releases a key on the text box.</summary>
        public event KeyEventHandler TextBoxPreviewKeyUp;
        #endregion

        #region Generic public properties
        /// <summary>Sets whether or not show error messages when Value is out of range. Default value is false.</summary>
        public bool ShowValueOutOfRangeErrors { get; set; } = false;

        private bool allowNegative = true;
        /// <summary>Sets whether or not the numeric up/down control should allow negative numbers. Default value is true.</summary>
        public bool AllowNegative
        {
            get { return allowNegative; }
            set
            {
                allowNegative = value;
                if (!value && minValue < 0) minValue = 0;
                numberTxt.Text = minValue.ToString();
            }
        }

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
                    cancelToken();//showing a message while the user is holding down a button will interrupt the mouse down event without triggering the mouse up event, this needs to be here otherwise the incrementer/decrementer will run forever
                    if(numberTxt.Text != "-" && numberTxt.Text != "")
                        MessageBox.Show("Error parsing numberTxt.Text.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    
                    numberTxt.Text = MinValue.ToString();
                    return MinValue -1;//it's complicated to explain, but the -1 is needed here (mainly when the action that called the get was ++Value)
                }
                catch (OverflowException)
                {
                    if (numberTxt.Text.Length > 0 && numberTxt.Text[0] == '-')
                    {
                        numberTxt.Text = MinValue.ToString();
                        return MinValue - 1;//it's complicated to explain, but the -1 is needed here (mainly when the action that called the get was ++Value)
                    }
                    else
                    {
                        numberTxt.Text = MaxValue.ToString();
                        return MaxValue + 1;//it's complicated to explain, but the +1 is needed here (mainly when the action that called the get was --Value)
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            set
            {
                if (value >= MinValue)
                {
                    if (value <= maxValue)
                    {
                        if (value >= 0 || AllowNegative)
                            numberTxt.Text = value.ToString();
                        else
                        {
                            numberTxt.Text = MinValue.ToString();
                            if (ShowValueOutOfRangeErrors) MessageBox.Show("Value cannot be negative. Negative numbers are not allowed unless AllowNegative is set to true.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        numberTxt.Text = MaxValue.ToString();
                        if (ShowValueOutOfRangeErrors) MessageBox.Show("Value is out of range. Value must be lesser or equal to MaxValue.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    numberTxt.Text = MinValue.ToString();
                    if (ShowValueOutOfRangeErrors) MessageBox.Show("Value is out of range. Value must be great or equal to MinValue.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }     
            }
        }

        private int maxValue = 2147483647;
        /// <summary>Maximum value the numeric text box can hold. Default value is 2,147,483,647.</summary>
        public int MaxValue
        {
            get { return maxValue; }
            set
            {
                if (value > MinValue)
                    if (value >= 0 || AllowNegative)//checks if negative numbers are allowed
                        maxValue = value;
                    else
                        MessageBox.Show("MaxValue cannot be negative. Negative numbers are not allowed.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("MaxValue cannot be lower or equal to MinValue.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);

                numberTxt.Text = MinValue.ToString();
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

                numberTxt.Text = minValue.ToString();
            }
        }
        #endregion

        #region Increment/decrement number
        private CancellationTokenSource cancelTokenSrc = new CancellationTokenSource();
        private bool isHoldingDownButton = false;
        private byte gradualSpeedCycles = 0;//used to tell how many cycles have passed, 1 cycle = 1 call to updateGradualSpeed(). gradualSpeedModifier only starts to be increased once a certain threshold of cycles have been passed.
        private byte gradualSpeedModifier = 0;//used gradually change in real time the speed of the increment/decrement of the number when holding down the button
        private const byte c_maxGradualSpeedIndex = 10;//only used to avoid magic numbers
        private const byte c_maxGradualSpeedModifier = 100;//only used to avoid magic numbers

        private byte gradualSpeedIndex = 10;
        /// <summary>Sets how fast the numbers will increase the longer the user hold down the button. Accepts values betweeen 1 and 10.
        /// 1 means that the gradual increase in speed is disabled and 2 is the slowest value possible. Default value is 10.</summary>
        public byte GradualSpeedIndex
        {
            get { return gradualSpeedIndex; }
            set
            {
                if (value > 0 && value <= 10)
                    gradualSpeedIndex = value;
                else
                    MessageBox.Show("Value out of range. Only values between 1 and 10 can be assigned to GradualSpeedIndex.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>Sets whether or not holding down the button will increase/decrease the number rapidly. Default value is true.</summary>
        public bool HoldDownToIncrease { get; set; } = true;

        private byte holdDownSpeed = 6;
        /// <summary>The speed which the number will increase/decrease when the user holds down the button (only works if HoldDownToIncrease is set to true). Can only accept values between 1 and 100. Default value is 6.</summary>
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

        private async void increaseNumber()
        {
            isHoldingDownButton = true;
            gradualSpeedModifier = 0;//resets the modifier
            gradualSpeedCycles = gradualSpeedIndex;//makes it start on a higher cycle so that the greater gradualSpeedIndex is, the fast the gradual increase in speed will occur

            while (isHoldingDownButton)
            {
                ++Value;//this must come before the await otherwise the number won't increase if the mouse/up event action were lesser than 1000/holdDownSpeed in milliseconds
                if (gradualSpeedIndex > 1) updateGradualSpeed();//1 = no gradual speed increase

                try
                {
                    await Task.Delay(1000/(holdDownSpeed+gradualSpeedModifier), cancelTokenSrc.Token);//theorically speaking the max value that (holdDownSpeed+gradualSpeedModifier) can have is 200 and a minimum delay of 5 milliseconds
                }
                catch (TaskCanceledException ex)
                {
                }
            }
        }

        private void updateGradualSpeed()
        {
            if (gradualSpeedModifier <= c_maxGradualSpeedModifier)
            {
                if (gradualSpeedCycles == c_maxGradualSpeedIndex)
                {
                    ++gradualSpeedModifier;
                    gradualSpeedCycles = gradualSpeedIndex;//makes it start on a higher cycle so that the greater gradualSpeedIndex is, the fast the gradual increase in speed will occur
                }
                else
                    ++gradualSpeedCycles;
            }
            //else
            //{
            //    MessageBox.Show("gradualSpeedModifier max value reached!");
            //}
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
            gradualSpeedModifier = 0;//resets the modifier
            gradualSpeedCycles = gradualSpeedIndex;//makes it start on a higher cycle so that the greater gradualSpeedIndex is, the fast the gradual increase in speed will occur

            while (isHoldingDownButton)
            {
                --Value;//this must come before the await otherwise the number won't increase if the mouse/up event action were lesser than 1000/holdDownSpeed in milliseconds
                if (gradualSpeedIndex > 1) updateGradualSpeed();//1 = no gradual speed increase

                try
                {
                    await Task.Delay(1000 / (holdDownSpeed + gradualSpeedModifier), cancelTokenSrc.Token);//theorically speaking the max value that (holdDownSpeed+gradualSpeedModifier) can have is 200 and a minimum delay of 5 milliseconds
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

        private void upButton_Click(object sender, RoutedEventArgs e)
        {
            if (!HoldDownToIncrease) ++Value;
        }

        private void downButton_Click(object sender, RoutedEventArgs e)
        {
            if (!HoldDownToIncrease) --Value;
        }

        #endregion
    }
}
