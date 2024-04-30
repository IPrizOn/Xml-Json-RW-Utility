using System.Windows;
using System.Windows.Media;

namespace Xml_Json_RW_Utility.FunctionalWindows
{
    public partial class InfoWindow : Window
    {
        // Диалоговое окно с информацией об успехе/предупреждении/ошибке
        public InfoWindow(string message, SolidColorBrush colorBrush)
        {
            InitializeComponent();

            // Отображение информации и соответствующего для неё цвета
            labelMessageText.Text = message;
            borderMessage.Background = colorBrush;
        }

        // Закрытие окна
        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
