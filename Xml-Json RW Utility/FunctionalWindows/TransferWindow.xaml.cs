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

        // Диалоговое окно конвертации файла
        public TransferWindow(string fileType)
        {
            InitializeComponent();

            openFileDialog = new OpenFileDialog();

            // Определение типа файла для работы и соответствующего оформления для него
            FileObject.fileType = fileType;
            if (FileObject.fileType.Equals(".xml"))
            {
                buttonSelectFile.Background = Brushes.PaleGreen;
                labelTypeTransfer.Content = "Выберите xml файл для конвертации";
                openFileDialog.Filter = "XML Files (*.xml)|*.xml";
            }
            else
            {
                buttonSelectFile.Background = Brushes.Gold;
                labelTypeTransfer.Content = "Выберите json файл для конвертации";
                openFileDialog.Filter = "JSON Files (*.json)|*.json";
            }
        }

        // Окно выбора файла и отображение его имени
        private void ButtonSelectFile_Click(object sender, RoutedEventArgs e)
        {
            if (openFileDialog.ShowDialog() == true)
            {
                if(FileObject.fileType.Equals(".xml"))
                {
                    labelFileName.Content = openFileDialog.SafeFileName.Replace(".xml", "");
                    labelFileName.Foreground = Brushes.PaleGreen;
                }
                else
                {
                    labelFileName.Content = openFileDialog.SafeFileName.Replace(".json", "");
                    labelFileName.Foreground = Brushes.Gold;
                }
                buttonAccept.IsEnabled = true;
            }
        }

        // Подтверждение и выбор перевода файла
        private void ButtonConfirm_Click(object sender, RoutedEventArgs e)
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
                string json = File.ReadAllText(openFileDialog.FileName);

                XmlDocument doc = JsonConvert.DeserializeXmlNode(json, "Root");

                string xmlFilePath = Path.ChangeExtension(openFileDialog.FileName, ".xml");
                doc.Save(xmlFilePath);

                XDocument xdoc = XDocument.Load(xmlFilePath);
                XElement rootElement = xdoc.Element("Root");
                xdoc.Element("Root").Remove();
                rootElement.Elements().ToList().ForEach(e => xdoc.Add(e));

                xdoc.Save(xmlFilePath);

                File.Delete(openFileDialog.FileName);
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
                XDocument xml = XDocument.Load(openFileDialog.FileName);

                XDocument cleanedXml = new XDocument(xml.Root);
                string cleanedXmlString = cleanedXml.ToString(SaveOptions.DisableFormatting);

                string json = JsonConvert.SerializeXNode(XDocument.Parse(cleanedXmlString));

                string jsonFilePath = Path.ChangeExtension(openFileDialog.FileName, ".json");
                File.WriteAllText(jsonFilePath, json);

                File.Delete(openFileDialog.FileName);
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
