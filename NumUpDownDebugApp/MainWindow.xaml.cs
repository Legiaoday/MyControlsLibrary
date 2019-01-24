using System.Windows;
using System.Windows.Input;

namespace NumUpDownDebugApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            nUpDown.TextBoxPreviewKeyDown += textBox_PreviewKeyDown;
            nUpDown.TextBoxPreviewKeyUp += textBox_PreviewKeyUp;
            nUpDown.AllowNegative = true;
            nUpDown.IsTextBoxReadOnly = false;
            nUpDown.ShowValueOutOfRangeErrors = false;
            nUpDown.HoldDownToIncrease = true;
            nUpDown.HoldDownSpeed = 6;
            nUpDown.GradualSpeedIndex = 10;
            nUpDown.MaxValue = 1000;
            nUpDown.MinValue = -1000;
            nUpDown.Value = 5;//setting nUpDown.MinValue to 1 also sets nUpDown.Value to 1
        }

        private void textBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void textBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {

        }
    }
}
