using System.Windows.Controls;
using System.Windows.Media;
using Xml_Json_RW_Utility.AppDataFile;
using Xml_Json_RW_Utility.FunctionalWindows;

namespace Xml_Json_RW_Utility.FunctionalPages
{
    public partial class HomePage : Page
    {
        public HomePage(string fileType)
        {
            InitializeComponent();

            // Определение оформления
            FileObject.fileType = fileType;
            if (FileObject.fileType.Equals(".xml"))
            {
                DockPanel.SetDock(btnSwitchType, Dock.Left);
                btnSwitchType.Background = Brushes.PaleGreen;
                borderUI.Background = Brushes.PaleGreen;
                btnTextTransfer.Content = "xml в json";        
            }
            else
            {
                DockPanel.SetDock(btnSwitchType, Dock.Right);
                btnSwitchType.Background = Brushes.Gold;
                borderUI.Background = Brushes.Gold;
                btnTextTransfer.Content = "json в xml";
            }
        }

        // На страницу Чтения/Записи
        private void ButtonToWrite(object sender, System.Windows.RoutedEventArgs e)
        {
            HomeObjects.frameHome.Navigate(new WritePage(FileObject.fileType));
        }

        // Диалоговое окно перевода
        private void ButtonToTransfer(object sender, System.Windows.RoutedEventArgs e)
        {
            TransferWindow transferWindow = new TransferWindow(FileObject.fileType);
            transferWindow.ShowDialog();
        }     

        // Переключение на работу с Json
        private void ButtonSwitcher_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            DockPanel.SetDock(btnSwitchType, Dock.Right);
            FileObject.fileType = ".json";
            btnSwitchType.Background = Brushes.Gold;
            borderUI.Background = Brushes.Gold;
            HomeObjects.labelHome.Foreground = Brushes.Gold;
            btnTextTransfer.Content = "json в xml";
        }

        // Переключение на работу с Xml
        private void ButtonSwitcher_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            DockPanel.SetDock(btnSwitchType, Dock.Left);
            FileObject.fileType = ".xml";
            btnSwitchType.Background = Brushes.PaleGreen;
            borderUI.Background = Brushes.PaleGreen;
            HomeObjects.labelHome.Foreground = Brushes.PaleGreen;
            btnTextTransfer.Content = "xml в json";
        }
    }
}
