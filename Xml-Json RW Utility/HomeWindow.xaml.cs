using System.Windows;
using System.Windows.Media;
using Xml_Json_RW_Utility.AppDataFile;
using Xml_Json_RW_Utility.FunctionalPages;

namespace Xml_Json_RW_Utility
{
    public partial class HomeWindow : Window
    {
        public HomeWindow()
        {
            InitializeComponent();

            // Задание начального окна и оформления для него
            HomeObjects.frameHome = frmHome;
            frmHome.Navigate(new HomePage(FileObject.fileType));

            HomeObjects.labelHome = lbHome;
            lbHome.Foreground = Brushes.PaleGreen;
        }

        // Выход из приложения
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // Возможность передвигать окно
        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}