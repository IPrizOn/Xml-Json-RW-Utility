using System.Windows;
using System.Windows.Media;

namespace Xml_Json_RW_Utility.FunctionalWindows
{
    public partial class InfoWindow : Window
    {
        public InfoWindow(string message, SolidColorBrush colorBrush)
        {
            InitializeComponent();

            labelMessageText.Text = message;
            borderMessage.Background = colorBrush;
        }

        // Закрытие окна
        private void ButtonOK(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
