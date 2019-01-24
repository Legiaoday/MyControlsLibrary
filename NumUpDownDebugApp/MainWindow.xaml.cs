﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NumUpDownDebugApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            nUpDown.AllowNegative = true;
            nUpDown.IsTextBoxReadOnly = true;
            nUpDown.ShowValueOutOfRangeErrors = false;
            nUpDown.HoldDownToIncrease = true;
            nUpDown.HoldDownSpeed = 6;
            nUpDown.GradualSpeedIndex = 10;
            nUpDown.MaxValue = 100;
            nUpDown.MinValue = 1;
            //nUpDown.Value = 1;//setting nUpDown.MinValue to 1 also sets nUpDown.Value to 1
        }
    }
}
