using System.Windows;
using System.Windows.Media;
using Xml_Json_RW_Utility.AppDataFile;
using Xml_Json_RW_Utility.FunctionalPages;

namespace Xml_Json_RW_Utility
{
    public partial class HomeWindow : Window
    {
        // Главное окно программы, в котором размещается контент в виде страниц
        public HomeWindow()
        {
            InitializeComponent();

            // Задание начальной страницы и оформления для неё
            HomeObjects.frameHome = frmHome;
            frmHome.Navigate(new HomePage(FileObject.fileType));

            HomeObjects.labelHome = lbHome;
            lbHome.Foreground = Brushes.PaleGreen;
        }

        // Завершение программы
        private void ButtonExit_Click(object sender, RoutedEventArgs e)
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