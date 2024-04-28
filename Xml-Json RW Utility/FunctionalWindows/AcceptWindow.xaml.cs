using System.Windows;
using System.Windows.Media;
using Xml_Json_RW_Utility.AppDataFile;

namespace Xml_Json_RW_Utility.FunctionalWindows
{
    public partial class AcceptWindow : Window
    {
        public AcceptWindow(string message, SolidColorBrush colorBrush)
        {
            InitializeComponent();

            labelMessageText.Text = message;
            borderMessage.Background = colorBrush;
        }

        private void ButtonNo(object sender, RoutedEventArgs e)
        {
            MessageBoxObject.resultMessage = MessageBoxResult.No;

            this.Close();
        }

        private void ButtonYes(object sender, RoutedEventArgs e)
        {
            MessageBoxObject.resultMessage = MessageBoxResult.Yes;

            this.Close();
        }
    }
}
