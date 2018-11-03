using System.Windows;
using MiscFunctionsLibrary;

namespace XMLHandlerDebugApp
{
    public partial class MainWindow : Window
    {
        XMLSettings settings;

        public MainWindow()
        {
            InitializeComponent();
            LoadXML();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            WriteXML();
        }

        private void LoadXML()
        {
            settings = XMLHandler.LoadConfigXML("seriesConfig.xml");

            if (settings != null)
            {
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
                            if(item.Value == "Maximized") this.WindowState = WindowState.Maximized;
                            else this.WindowState = WindowState.Normal;
                            break;
                    }
                }
            }
        }

        private void WriteXML()
        {
            XMLSettings settings = new XMLSettings();
            settings.AddNewItem("WindowXPosition", this.Left.ToString());
            settings.AddNewItem("WindowYPosition", this.Top.ToString());
            settings.AddNewItem("WindowXSize", this.Width.ToString());
            settings.AddNewItem("WindowYSize", this.Height.ToString());
            settings.AddNewItem("WindowState", this.WindowState.ToString());

            XMLHandler.WriteConfigXML("seriesConfig.xml", settings);
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
            if (isRemoved) MessageBox.Show("WindowYPosition remove!");
        }
    }
}
