using System.Windows;
using System.Windows.Media;
using Xml_Json_RW_Utility.AppDataFile;

namespace Xml_Json_RW_Utility.FunctionalWindows
{
    public partial class AcceptWindow : Window
    {
        // Диалоговое окно подтверждения изменений
        public AcceptWindow(string message, SolidColorBrush colorBrush)
        {
            InitializeComponent();

            // Отобржаение подтверждения в окне и соответствующего для неё цвета
            labelMessageText.Text = message;
            borderMessage.Background = colorBrush;
        }

        // Отмена изменений
        private void ButtonNo_CLick(object sender, RoutedEventArgs e)
        {
            MessageBoxObject.resultMessage = MessageBoxResult.No;

            this.Close();
        }

        // Применение изменений
        private void ButtonYes_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxObject.resultMessage = MessageBoxResult.Yes;

            this.Close();
        }
    }
}
