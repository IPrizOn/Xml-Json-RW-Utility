using System.Windows;
using System.Windows.Media;
using System.Xml;

namespace Xml_Json_RW_Utility.FunctionalWindows
{
    public partial class EditWindow : Window
    {
        public string EditNode { get; set; }

        // Диалоговое окно изменения xml элемента
        public EditWindow(XmlNode node, string tag)
        {
            InitializeComponent();

            // Отображение изменяемого элемента и соответствующего для него цвета
            labelElementName.Foreground = Brushes.PaleGreen;
            labelElementName.Text = tag;

            EditNode = node.InnerText;
            textBoxElementValue.Text = node.InnerText;
        }

        // Диалоговое окно изменения json элемента
        public EditWindow(string node, string tag)
        {
            InitializeComponent();

            // Отображение изменяемого элемента и соответствующего для него цвета
            labelElementName.Foreground = Brushes.Gold; 
            labelElementName.Text = tag;

            EditNode = node;
            textBoxElementValue.Text= node;
        }

        // Сохранение изменений
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            EditNode = textBoxElementValue.Text;
            Close();
        }
    }
}
