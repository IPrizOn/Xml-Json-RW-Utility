using System.Windows;
using System.Windows.Media;
using System.Xml;

namespace Xml_Json_RW_Utility.FunctionalWindows
{
    public partial class EditWindow : Window
    {
        public string EditNode { get; set; }

        public EditWindow(XmlNode node)
        {
            InitializeComponent();

            labelElementName.Foreground = Brushes.PaleGreen;
            labelElementName.Text = node.InnerText;

            EditNode = node.InnerText;
            textBoxElementValue.Text = node.InnerText;
        }

        public EditWindow(string node)
        {
            InitializeComponent();

            labelElementName.Foreground = Brushes.Gold; 
            labelElementName.Text = node;

            EditNode = node;
            textBoxElementValue.Text= node;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            EditNode = textBoxElementValue.Text;
            Close();
        }
    }
}
