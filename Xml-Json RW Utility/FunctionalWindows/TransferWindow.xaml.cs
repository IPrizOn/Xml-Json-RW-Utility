using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Win32;
using Newtonsoft.Json;
using Xml_Json_RW_Utility.AppDataFile;

namespace Xml_Json_RW_Utility.FunctionalWindows
{
    public partial class TransferWindow : Window
    {
        private OpenFileDialog openFileDialog;
        private string filePath;
        private string fileName;

        public TransferWindow(string fileType)
        {
            InitializeComponent();

            openFileDialog = new OpenFileDialog();

            // Определение типа файла для работы и соответствующего оформления для него
            FileObject.fileType = fileType;
            if (FileObject.fileType.Equals(".xml"))
            {
                btnSelectFile.Background = Brushes.PaleGreen;
                labelTypeTransfer.Content = "Выберите xml файл для перевода";
                openFileDialog.Filter = "XML Files (*.xml)|*.xml";
            }
            else
            {
                btnSelectFile.Background = Brushes.Gold;
                labelTypeTransfer.Content = "Выберите json файл для перевода";
                openFileDialog.Filter = "JSON Files (*.json)|*.json";
            }
        }

        // Окно выбора файла
        private void ButtonSelectFile(object sender, RoutedEventArgs e)
        {
            // Отображение окна выбора файла и названия файла
            if (openFileDialog.ShowDialog() == true)
            {
                fileName = openFileDialog.SafeFileName;
                if(FileObject.fileType.Equals(".xml"))
                {
                    labelFileName.Content = fileName.Replace(".xml", "");
                    labelFileName.Foreground = Brushes.PaleGreen;
                }
                else
                {
                    labelFileName.Content = fileName.Replace(".json", "");
                    labelFileName.Foreground = Brushes.Gold;
                }
                btnAccept.IsEnabled = true;
            }
        }

        // Подтверждение и выбор перевода файла
        private void ButtonConfirm(object sender, RoutedEventArgs e)
        {
            if (FileObject.fileType.Equals(".xml"))
            {
                ChangeToJson();
            }
            else
            {
                ChangeToXml();
            }
        }

        // Перевод JSON в XML
        private void ChangeToXml()
        {
            try
            {
                filePath = openFileDialog.FileName;

                string json = File.ReadAllText(filePath);

                XmlDocument doc = JsonConvert.DeserializeXmlNode(json, "Root");

                string xmlFilePath = Path.ChangeExtension(filePath, ".xml");
                doc.Save(xmlFilePath);

                XDocument xdoc = XDocument.Load(xmlFilePath);
                XElement rootElement = xdoc.Element("Root");
                xdoc.Element("Root").Remove();
                rootElement.Elements().ToList().ForEach(e => xdoc.Add(e));

                xdoc.Save(xmlFilePath);

                File.Delete(filePath);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                InfoWindow infoWindow = new InfoWindow("Не удалось загрузить файл.", Brushes.Firebrick);
                infoWindow.ShowDialog();

                Console.WriteLine(ex.Message);
            }

            Close();
        }

        // Перевод XML в JSON
        private void ChangeToJson()
        {
            try
            {
                filePath = openFileDialog.FileName;

                XDocument xml = XDocument.Load(filePath);

                XDocument cleanedXml = new XDocument(xml.Root);
                string cleanedXmlString = cleanedXml.ToString(SaveOptions.DisableFormatting);

                string json = JsonConvert.SerializeXNode(XDocument.Parse(cleanedXmlString));

                string jsonFilePath = Path.ChangeExtension(filePath, ".json");
                File.WriteAllText(jsonFilePath, json);

                File.Delete(filePath);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                InfoWindow infoWindow = new InfoWindow("Не удалось загрузить файл.", Brushes.Firebrick);
                infoWindow.ShowDialog();

                Console.WriteLine(ex.Message);
            }

            Close();
        }
    }
}
