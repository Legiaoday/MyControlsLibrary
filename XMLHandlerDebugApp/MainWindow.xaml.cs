using System.Windows;
using MiscFunctionsLibrary;

namespace XMLHandlerDebugApp
{
    public partial class MainWindow : Window
    {
        XMLSettings settings = new XMLSettings();

        public MainWindow()
        {
            InitializeComponent();
            LoadXML();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveXML();
        }

        private void LoadXML()
        {
            settings.LoadConfigXML("seriesConfig.xml");

            foreach(XMLSettingItem item in settings.Items)
            {
                switch (item.Name)
                {
                    case "WindowXPosition":
                        this.Left = double.Parse(item.Value);
                        break;
                    case "WindowYPosition":
                        this.Top = double.Parse(item.Value);
                        break;
                    case "WindowXSize":
                        this.Width = double.Parse(item.Value);
                        break;
                    case "WindowYSize":
                        this.Height = double.Parse(item.Value);
                        break;
                    case "WindowState":
                        if(item.Value == "Maximized")
                            this.WindowState = WindowState.Maximized;
                        else
                            this.WindowState = WindowState.Normal;
                        break;
                }
            }
        }

        private void SaveXML()
        {
            settings.UpdateItem("WindowXPosition", this.Left.ToString(), true);
            settings.UpdateItem("WindowYPosition", this.Top.ToString(), true);
            settings.UpdateItem("WindowXSize", this.Width.ToString(), true);
            settings.UpdateItem("WindowYSize", this.Height.ToString(), true);
            settings.UpdateItem("WindowState", this.WindowState.ToString(), true);

            settings.WriteConfigXML("seriesConfig.xml");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string yPos = settings.GetItemValue("WindowYPosition");
            if(yPos != null) MessageBox.Show(yPos);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int yIndex = settings.GetItemIndex("WindowYPosition");
            if (yIndex != -1) MessageBox.Show(yIndex.ToString());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            bool isRemoved = settings.RemoveItem("WindowYPosition");
            if (isRemoved) MessageBox.Show("WindowYPosition removed!");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            XMLSettingItem item = settings.GetItem("WindowYPosition");

            if (item != null)
            {
                MessageBox.Show("Original item value: " + item.Value);
                item.Value = (System.Convert.ToDouble(item.Value) + 157).ToString();
                MessageBox.Show("New item value: " + settings.GetItemValue("WindowYPosition"));
            }
        }
    }
}