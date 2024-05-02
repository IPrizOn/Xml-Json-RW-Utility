using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
using Xml_Json_RW_Utility.AppDataFile;
using Xml_Json_RW_Utility.FunctionalWindows;

namespace Xml_Json_RW_Utility.FunctionalPages
{
    public partial class WriteReadPage : Page
    {
        private OpenFileDialog openFileDialog;
        private Dictionary<string, List<string>> tagAndElements;
        private XmlDocument savedXmlDocument;
        private string savedXmlPath;
        private JObject savedJsonDocument;
        private string savedJsonPath;
        private bool isXml;

        // Страница чтения/записи
        public WriteReadPage(string fileType)
        {
            InitializeComponent();

            openFileDialog = new OpenFileDialog();
            tagAndElements = new Dictionary<string, List<string>>();

            savedXmlDocument = new XmlDocument();
            savedJsonDocument = new JObject();

            // Определение типа файла для работы и соответствующего оформления окна для него
            isXml = StringCheck.IsXmlFile(FileObject.fileType);
            if (isXml)
            {
                openFileDialog.Filter = "XML Files (*.xml)|*.xml";
                labelWriteRead.Content += " XML";
                borderFileInfo.Background = Brushes.PaleGreen;
                buttonSelectFile.Background = Brushes.PaleGreen;
                buttonAcceptChanges.Background = Brushes.PaleGreen;
                labelFileName.Foreground = Brushes.PaleGreen;
            }
            else
            {
                openFileDialog.Filter = "JSON Files (*.json)|*.json";
                labelWriteRead.Content += " JSON";
                borderFileInfo.Background = Brushes.Gold;
                buttonSelectFile.Background = Brushes.Gold;
                buttonAcceptChanges.Background = Brushes.Gold;
                labelFileName.Foreground = Brushes.Gold;
            }
        }

        // Возврат на главную
        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            HomeObjects.frameHome.Navigate(new HomePage(FileObject.fileType));
        }

        // Сброс информации в файле, если были изменения
        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            AcceptWindow acceptWindow = new AcceptWindow("Вы уверены, что хотите сбросить?", Brushes.Orange);
            acceptWindow.ShowDialog();

            if (MessageBoxObject.resultMessage == MessageBoxResult.Yes)
            {
                if (isXml)
                {
                    try
                    {
                        savedXmlDocument.Save(savedXmlPath);
                    }
                    catch (Exception ex)
                    {
                        InfoWindow infoWindowError = new InfoWindow("Не удалось сбросить изменения.", Brushes.Firebrick);
                        infoWindowError.ShowDialog();

                        Console.WriteLine(ex.Message);
                    }

                    LoadXml();
                }
                else
                {
                    try
                    {
                        File.WriteAllText(savedJsonPath, savedJsonDocument.ToString());
                    }
                    catch (Exception ex)
                    {
                        InfoWindow infoWindowError = new InfoWindow("Не удалось сбросить изменения.", Brushes.Firebrick);
                        infoWindowError.ShowDialog();

                        Console.WriteLine(ex.Message);
                    }

                    LoadJson();
                }

                textBoxTagPost.Text = null;
                textBoxElementPost.Text = null;
                buttonClear.IsEnabled = false;
                buttonAcceptChanges.IsEnabled = false;

                InfoWindow infoWindow = new InfoWindow("Информация успешно сброшена!", Brushes.LimeGreen);
                infoWindow.ShowDialog();
            }

            MessageBoxObject.resultMessage = MessageBoxResult.No;
        }

        // Очистка содержимого всех полей на странице
        private void ClearAll()
        {
            openFileDialog.FileName = "";
            labelFileName.Content = "";
            textBoxTagPost.Text = null;
            textBoxElementPost.Text = null;
            listBoxItemsList.Items.Clear();
            tagAndElements.Clear();
            comboBoxElementPostIn.ItemsSource = null;
            comboBoxTagDelete.ItemsSource = null;
            comboBoxElementDeleteFrom.ItemsSource = null;
            comboBoxElementDelete.Items.Clear();          
        }

        // Окно выбора файла и логика его загрузки
        private void ButtonSelectFile_Click(object sender, RoutedEventArgs e)
        {
            if (openFileDialog.ShowDialog() == true)
            {
                if (isXml)
                {
                    labelFileName.Content = openFileDialog.SafeFileName.Replace(".xml", "");

                    savedXmlDocument.Load(openFileDialog.FileName);
                    savedXmlPath = openFileDialog.FileName;
                }
                else
                {
                    labelFileName.Content = openFileDialog.SafeFileName.Replace(".json", "");

                    savedJsonDocument = JObject.Parse(File.ReadAllText(openFileDialog.FileName));
                    savedJsonPath = openFileDialog.FileName;
                }
            }

            if (isXml)
            {
                LoadXml();
            }
            else
            {
                LoadJson();
            }
        }

        // Загрузка XML файла
        private void LoadXml()
        {
            try
            {
                listBoxItemsList.Items.Clear();
                comboBoxElementPostIn.ItemsSource = null;
                comboBoxTagDelete.ItemsSource = null;
                comboBoxElementDeleteFrom.ItemsSource = null;
                comboBoxElementDelete.Items.Clear();
                tagAndElements.Clear();

                XDocument xDoc = XDocument.Load(openFileDialog.FileName);
                var groupedElements = xDoc.Root.Elements().GroupBy(element => element.Name.LocalName).Select(group => new
                {
                    ElementName = group.Key,
                    Elements = group.Nodes()
                });

                foreach (var group in groupedElements)
                {
                    StackPanel stackPanelMain = new StackPanel()
                    {
                        Orientation = Orientation.Vertical
                    };

                    tagAndElements.Add(group.ElementName, new List<string>());

                    StackPanel stackPanelTag = new StackPanel()
                    {
                        Orientation = Orientation.Horizontal
                    };

                    stackPanelTag.Children.Add(new ListBoxItem
                    {
                        Content = $"<{group.ElementName.ToString()}>"
                    });

                    stackPanelMain.Children.Add(stackPanelTag);

                    foreach (var element in group.Elements)
                    {
                        StackPanel stackPanelInner = new StackPanel()
                        {
                            Orientation = Orientation.Horizontal
                        };

                        stackPanelInner.Children.Add(new ListBoxItem
                        {
                            Content = element.ToString(),
                            Margin = new Thickness(10, 0, 0, 0)
                        });

                        Button buttonInner = new Button()
                        {
                            FontSize = 12,
                            FontWeight = FontWeights.Bold,
                            Background = Brushes.Transparent,
                            BorderThickness = new Thickness(0),
                            Content = "(изменить)",
                            Tag = group.ElementName + "." + element.ToString(),
                        };

                        buttonInner.Click += ButtonEdit_Click;
                        stackPanelInner.Children.Add(buttonInner);
                        stackPanelMain.Children.Add(stackPanelInner);

                        tagAndElements[group.ElementName].Add(element.ToString());

                    }

                    stackPanelMain.Children.Add(new ListBoxItem
                    {
                        Content = $"<{group.ElementName.ToString()}>"
                    });

                    listBoxItemsList.Items.Add(stackPanelMain);
                }

                comboBoxTagDelete.ItemsSource = tagAndElements.Keys;
                comboBoxElementPostIn.ItemsSource = tagAndElements.Keys;
                comboBoxElementDeleteFrom.ItemsSource = tagAndElements.Keys;
            }
            catch (ArgumentException ex)
            {
                ClearAll();

                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                ClearAll();

                InfoWindow infoWindow = new InfoWindow("Не удалось загрузить или обновить файл.", Brushes.Firebrick);
                infoWindow.ShowDialog();

                Console.WriteLine(ex.Message);
            }
        }       

        // Загрузка JSON файла
        private void LoadJson()
        {
            try
            {
                listBoxItemsList.Items.Clear();
                comboBoxElementPostIn.ItemsSource = null;
                comboBoxTagDelete.ItemsSource = null;
                comboBoxElementDeleteFrom.ItemsSource = null;
                comboBoxElementDelete.Items.Clear();
                tagAndElements.Clear();               

                string jsonText = File.ReadAllText(openFileDialog.FileName);
                JObject json = JObject.Parse(jsonText);

                foreach (var node in json)
                {
                    foreach(var tag in node.Value)
                    {
                        foreach(var item in tag)
                        {
                            StackPanel stackPanelMain = new StackPanel()
                            {
                                Orientation = Orientation.Vertical
                            };

                            tagAndElements.Add(item.Path.ToString().Split('.')[1], new List<string>());

                            StackPanel stackPanelTag = new StackPanel()
                            {
                                Orientation = Orientation.Horizontal
                            };

                            stackPanelTag.Children.Add(new ListBoxItem
                            {
                                Content = $"\"{item.Path.ToString().Split('.')[1]}\":"
                            });

                            stackPanelMain.Children.Add(stackPanelTag);

                            foreach (var element in tag)
                            {
                                foreach (var value in element)
                                {
                                    StackPanel stackPanelInner = new StackPanel()
                                    {
                                        Orientation = Orientation.Horizontal
                                    };

                                    stackPanelInner.Children.Add(new ListBoxItem
                                    {
                                        Content = value.ToString(),
                                        Margin = new Thickness(10, 0, 0, 0)
                                    });

                                    Button buttonInner = new Button()
                                    {
                                        FontSize = 12,
                                        FontWeight = FontWeights.Bold,
                                        Background = Brushes.Transparent,
                                        BorderThickness = new Thickness(0),
                                        Content = "(изменить)",
                                        Tag = item.Path.ToString().Split('.')[1] + "." + value.ToString()
                                    };

                                    buttonInner.Click += ButtonEdit_Click;
                                    stackPanelInner.Children.Add(buttonInner);
                                    stackPanelMain.Children.Add(stackPanelInner);

                                    tagAndElements[item.Path.ToString().Split('.')[1]].Add(value.ToString());
                                }
                            }

                            listBoxItemsList.Items.Add(stackPanelMain);
                        }
                    }
                }

                comboBoxTagDelete.ItemsSource = tagAndElements.Keys;
                comboBoxElementPostIn.ItemsSource = tagAndElements.Keys;
                comboBoxElementDeleteFrom.ItemsSource = tagAndElements.Keys;
            }
            catch(ArgumentException ex)
            {
                ClearAll();

                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                ClearAll();

                InfoWindow infoWindow = new InfoWindow("Не удалось загрузить или обновить файл.", Brushes.Firebrick);
                infoWindow.ShowDialog();

                Console.WriteLine(ex.Message);
            }
        }

        // Применение изменений в файле
        private void ButtonAcceptChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Добавление тега
                if (!string.IsNullOrEmpty(textBoxTagPost.Text))
                {
                    if (StringCheck.IsValidName(textBoxTagPost.Text))
                    {
                        if (StringCheck.IsTagExist(textBoxTagPost.Text, tagAndElements))
                        {
                            if (isXml)
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.Load(openFileDialog.FileName);
                                XmlNode rootNode = doc.DocumentElement;

                                XmlNode newTag = doc.CreateNode(XmlNodeType.Element, textBoxTagPost.Text, "");
                                newTag.InnerText = "";
                                rootNode.AppendChild(newTag);
                                doc.Save(openFileDialog.FileName);
                            }
                            else
                            {
                                JObject jsonObject = JObject.Parse(File.ReadAllText(openFileDialog.FileName));

                                JObject newNode = new JObject();
                                jsonObject["Root"][textBoxTagPost.Text] = newNode;

                                File.WriteAllText(openFileDialog.FileName, jsonObject.ToString());
                            }  
                        }
                        else
                        {
                            InfoWindow warningWindow = new InfoWindow($"Тег с именем {textBoxTagPost.Text} уже существует", Brushes.Orange);
                            warningWindow.ShowDialog();
                        }
                    }
                    else
                    {
                        InfoWindow warningWindow = new InfoWindow("Название тега не должно начинаться на цифру и содержать знаков препинания", Brushes.Orange);
                        warningWindow.ShowDialog();
                    }

                    textBoxTagPost.Text = null;
                }               

                // Удаление тега
                if (comboBoxTagDelete.SelectedItem != null)
                {
                    if (isXml)
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(openFileDialog.FileName);
                        XmlNode rootNode = doc.DocumentElement;
                        XmlNode tagNode = rootNode.SelectSingleNode(comboBoxTagDelete.SelectedItem.ToString());

                        if (tagNode != null)
                        {
                            rootNode.RemoveChild(tagNode);
                            doc.Save(openFileDialog.FileName);
                        } 
                    }
                    else
                    {
                        JObject jsonObject = JObject.Parse(File.ReadAllText(openFileDialog.FileName));

                        jsonObject["Root"].SelectToken(comboBoxTagDelete.SelectedItem.ToString()).Parent.Remove();

                        File.WriteAllText(openFileDialog.FileName, jsonObject.ToString());
                    }
                }

                // Добавление элемента в тег
                if (comboBoxElementPostIn.SelectedItem != null && !string.IsNullOrEmpty(textBoxElementPost.Text))
                {
                    if (StringCheck.IsValidName(textBoxElementPost.Text))
                    {                       
                        if (StringCheck.IsElementExist(isXml, textBoxElementPost.Text, tagAndElements[comboBoxElementPostIn.SelectedItem.ToString()]))
                        {
                            if (isXml)
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.Load(openFileDialog.FileName);
                                XmlNode rootNode = doc.DocumentElement;
                                XmlNode selectedElement = rootNode.SelectSingleNode(comboBoxElementPostIn.SelectedItem.ToString());

                                if (selectedElement != null)
                                {
                                    XmlElement newElement = doc.CreateElement(textBoxElementPost.Text);
                                    XmlText elementText = doc.CreateTextNode("");

                                    newElement.AppendChild(elementText);
                                    selectedElement.AppendChild(newElement);
                                    doc.Save(openFileDialog.FileName);
                                }
                            }
                            else
                            {
                                JObject jsonObject = JObject.Parse(File.ReadAllText(openFileDialog.FileName));

                                JObject selectedNodeObject = (JObject)jsonObject["Root"][comboBoxElementPostIn.SelectedItem.ToString()];
                                selectedNodeObject[textBoxElementPost.Text] = "";

                                File.WriteAllText(openFileDialog.FileName, jsonObject.ToString());
                            }  
                        }
                        else
                        {
                            InfoWindow warningWindow = new InfoWindow($"Элемент с именем {textBoxElementPost.Text} уже существует", Brushes.Orange);
                            warningWindow.ShowDialog();
                        }
                    }
                    else
                    {
                        InfoWindow warningWindow = new InfoWindow("Название элемента не должно начинаться на цифру и содержать знаков препинания", Brushes.Orange);
                        warningWindow.ShowDialog();
                    }

                    textBoxElementPost.Text = null;
                }

                // Удаление элемента из тега
                if (comboBoxElementDeleteFrom.SelectedItem != null && comboBoxElementDelete.SelectedItem != null)
                {
                    string elementString = StringCheck.ElementWithoutSymbols(isXml, comboBoxElementDelete.SelectedItem.ToString());
                    if (isXml)
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(openFileDialog.FileName);
                        XmlNode root = doc.DocumentElement;
                        XmlNode parentTag = root.SelectSingleNode(comboBoxElementDeleteFrom.SelectedItem.ToString());

                        if (parentTag != null)
                        {
                            XmlNode elementToRemove = parentTag.SelectSingleNode(elementString);

                            if (elementToRemove != null)
                            {
                                parentTag.RemoveChild(elementToRemove);
                                doc.Save(openFileDialog.FileName);
                            }
                        } 
                    }
                    else
                    {
                        JObject jsonObject = JObject.Parse(File.ReadAllText(openFileDialog.FileName));

                        JObject parentNode = (JObject)jsonObject["Root"][comboBoxElementDeleteFrom.SelectedItem.ToString()];

                        parentNode.Property(elementString).Remove();

                        File.WriteAllText(openFileDialog.FileName, jsonObject.ToString());
                    }
                }


                // Обновление информации на странице после изменений
                if (isXml)
                {
                    LoadXml();
                }
                else
                {
                    LoadJson();
                }

                buttonClear.IsEnabled = true;
            }
            catch (Exception ex)
            {
                InfoWindow errorWindow = new InfoWindow("Не удалось применить изменения.", Brushes.Firebrick);
                errorWindow.ShowDialog();

                Console.WriteLine(ex.Message);
            }           
        }      

        // Редактирование элементов тега
        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isXml)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(openFileDialog.FileName);
                  
                    Button button = (Button)sender;
                    string nodeName = button.Tag.ToString();

                    XmlNode root = doc.DocumentElement;

                    XmlNode parentTag = root.SelectSingleNode(nodeName.Split('.')[0]);
                    if (parentTag != null)
                    {
                        string elementString = StringCheck.ElementWithoutSymbols(isXml, nodeName.Split('.')[1]);

                        XmlNode elementToEdit = parentTag.SelectSingleNode(elementString);

                        EditWindow editWindow = new EditWindow(elementToEdit.InnerText, elementString);
                        editWindow.ShowDialog();

                        XmlNode elementNew = doc.CreateElement(elementString);
                        elementNew.InnerText = editWindow.EditNode;
                        parentTag.ReplaceChild(elementNew, elementToEdit);

                        doc.Save(openFileDialog.FileName);
                    }                                     

                    LoadXml();
                }
                else
                {
                    JObject jsonObject = JObject.Parse(File.ReadAllText(openFileDialog.FileName));

                    Button button = (Button)sender;
                    string nodeName = button.Tag.ToString();

                    JToken root = jsonObject.SelectToken("Root");

                    JToken parentTag = root.SelectToken(nodeName.Split('.')[0]);
                    if (parentTag != null)
                    {
                        string elementString = StringCheck.ElementWithoutSymbols(isXml, nodeName.Split('.')[1]);

                        JToken elementToEdit = parentTag.SelectToken(elementString);

                        EditWindow editWindow = new EditWindow(elementToEdit.ToString(), elementString);
                        editWindow.ShowDialog();

                        JValue elementNew = new JValue(editWindow.EditNode);
                        parentTag[elementString] = elementNew;

                        File.WriteAllText(openFileDialog.FileName, jsonObject.ToString());
                    }

                    LoadJson();
                }

                buttonClear.IsEnabled = true;
            }
            catch (Exception ex)
            {
                InfoWindow errorWindow = new InfoWindow("Не удалось изменить элемент.", Brushes.Firebrick);
                errorWindow.ShowDialog();

                Console.WriteLine(ex.Message);
            }
        }

        // Разблокировка кнопки применения изменений, если написан контент в TextBox'ах
        private void TextBoxPost_TextChanged(object sender, TextChangedEventArgs e)
        {
            buttonAcceptChanges.IsEnabled = (!string.IsNullOrWhiteSpace(textBoxTagPost.Text) || !string.IsNullOrWhiteSpace(textBoxElementPost.Text)) && !string.IsNullOrWhiteSpace(labelFileName.Content.ToString());
        }

        // Разблокировка кнопки применения изменений, если выбран контент в ComboBox'ах
        private void ComboBoxes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            buttonAcceptChanges.IsEnabled = true;
        }

        // Отображение подкатегорий после выбора категории в ComboBox для удаление элемента из тега
        private void ComboBoxElementDeleteFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comboBoxElementDelete.Items.Clear();

            if (comboBoxElementDeleteFrom.SelectedItem != null)
            {
                foreach (var element in tagAndElements[comboBoxElementDeleteFrom.SelectedItem.ToString()])
                {
                    comboBoxElementDelete.Items.Add(element);
                } 
            }

            buttonAcceptChanges.IsEnabled = true;
        }
    }
}
