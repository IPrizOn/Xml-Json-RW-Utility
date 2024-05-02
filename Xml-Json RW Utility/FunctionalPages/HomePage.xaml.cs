using System.Windows.Controls;
using System.Windows.Media;
using Xml_Json_RW_Utility.AppDataFile;
using Xml_Json_RW_Utility.FunctionalWindows;

namespace Xml_Json_RW_Utility.FunctionalPages
{
    public partial class HomePage : Page
    {
        // Начальная страница программы
        public HomePage(string fileType)
        {
            InitializeComponent();

            // Определение типа файла для работы и соответствующего оформления окна для него
            FileObject.fileType = fileType;
            if (FileObject.fileType.Equals(".xml"))
            {
                DockPanel.SetDock(buttonSwitchType, Dock.Left);
                buttonSwitchType.Background = Brushes.PaleGreen;
                borderUI.Background = Brushes.PaleGreen;
                buttonToTransfer.Content = "xml в json";        
            }
            else
            {
                DockPanel.SetDock(buttonSwitchType, Dock.Right);
                buttonSwitchType.Background = Brushes.Gold;
                borderUI.Background = Brushes.Gold;
                buttonToTransfer.Content = "json в xml";
            }
        }

        // На страницу Чтения/Записи
        private void ButtonToWriteRead(object sender, System.Windows.RoutedEventArgs e)
        {
            HomeObjects.frameHome.Navigate(new WriteReadPage(FileObject.fileType));
        }

        // Диалоговое окно конвертации
        private void ButtonToTransfer(object sender, System.Windows.RoutedEventArgs e)
        {
            TransferWindow transferWindow = new TransferWindow(FileObject.fileType);
            transferWindow.ShowDialog();
        }     

        // Переключение на работу с JSON
        private void ButtonSwitcher_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            DockPanel.SetDock(buttonSwitchType, Dock.Right);
            FileObject.fileType = ".json";
            buttonSwitchType.Background = Brushes.Gold;
            borderUI.Background = Brushes.Gold;
            HomeObjects.labelHome.Foreground = Brushes.Gold;
            buttonToTransfer.Content = "json в xml";
        }

        // Переключение на работу с XML
        private void ButtonSwitcher_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            DockPanel.SetDock(buttonSwitchType, Dock.Left);
            FileObject.fileType = ".xml";
            buttonSwitchType.Background = Brushes.PaleGreen;
            borderUI.Background = Brushes.PaleGreen;
            HomeObjects.labelHome.Foreground = Brushes.PaleGreen;
            buttonToTransfer.Content = "xml в json";
        }
    }
}
