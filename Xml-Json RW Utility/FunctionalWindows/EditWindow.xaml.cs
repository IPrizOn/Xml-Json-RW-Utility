using System.Windows;
using System.Windows.Media;
using System.Xml;
using Xml_Json_RW_Utility.AppDataFile;

namespace Xml_Json_RW_Utility.FunctionalWindows
{
    public partial class EditWindow : Window
    {
        public string EditNode { get; set; }
        private bool isXml;

        // Диалоговое окно изменения xml элемента
        public EditWindow(string node, string tag)
        {
            InitializeComponent();

            // Отображение изменяемого элемента и соответствующего для него цвета
            isXml = StringCheck.IsXmlFile(FileObject.fileType);
            if (isXml)
            {
                labelElementName.Foreground = Brushes.PaleGreen;                 
            }
            else
            {
                labelElementName.Foreground = Brushes.Gold;
            }

            labelElementName.Text = tag;
            textBoxElementValue.Text = node;
            EditNode = node;
        }

        // Сохранение изменений
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            EditNode = textBoxElementValue.Text;
            Close();
        }
    }
}
