﻿using Microsoft.Win32;
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
    public partial class WritePage : Page
    {
        private OpenFileDialog openFileDialog;
        private Dictionary<string, List<string>> tagAndElements;

        public WritePage(string fileType)
        {
            InitializeComponent();

            openFileDialog = new OpenFileDialog();
            tagAndElements = new Dictionary<string, List<string>>();

            // Определение типа файла для работы и соответствующего оформления для него
            FileObject.fileType = fileType;
            if (FileObject.fileType.Equals(".xml"))
            {
                openFileDialog.Filter = "XML Files (*.xml)|*.xml";
                labelWrite.Content += " XML";
                borderFileInfo.Background = Brushes.PaleGreen;
                buttonSelectFile.Background = Brushes.PaleGreen;
                buttonAcceptChanges.Background = Brushes.PaleGreen;
                labelFileName.Foreground = Brushes.PaleGreen;
            }
            else
            {
                openFileDialog.Filter = "JSON Files (*.json)|*.json";
                labelWrite.Content += " JSON";
                borderFileInfo.Background = Brushes.Gold;
                buttonSelectFile.Background = Brushes.Gold;
                buttonAcceptChanges.Background = Brushes.Gold;
                labelFileName.Foreground = Brushes.Gold;
            }
        }

        // Возврат на главную
        private void ButtonBack(object sender, RoutedEventArgs e)
        {
            HomeObjects.frameHome.Navigate(new HomePage(FileObject.fileType));
            if(FileObject.fileType.Equals(".xml"))
            {
                HomeObjects.labelHome.Foreground = Brushes.PaleGreen;
            }
            else
            {
                HomeObjects.labelHome.Foreground = Brushes.Gold;
            }
        }

        // Сброс информации на странице
        private void ButtonClear(object sender, RoutedEventArgs e)
        {
            AcceptWindow acceptWindow = new AcceptWindow("Вы уверены, что хотите сбросить?", Brushes.Orange);
            acceptWindow.ShowDialog();

            if (MessageBoxObject.resultMessage == MessageBoxResult.Yes)
            {
                ClearAll();
                buttonClear.IsEnabled = false;

                InfoWindow infoWindow = new InfoWindow("Информация успешно сброшена!", Brushes.LimeGreen);
                infoWindow.ShowDialog();
            }

            MessageBoxObject.resultMessage = MessageBoxResult.No;
        }

        // Очистка всего содержимого на странице
        private void ClearAll()
        {
            openFileDialog.FileName = "";
            labelFileName.Content = "";
            textBoxTagPost.Text = "";
            textBoxElementPost.Text = "";
            listBoxItemsList.Items.Clear();
            tagAndElements.Clear();
            comboBoxElementPostIn.ItemsSource = null;
            comboBoxTagDelete.ItemsSource = null;
            comboBoxElementDeleteFrom.ItemsSource = null;
            comboBoxElementDelete.Items.Clear();
        }

        // Окно выбора файла и логика его загрузки
        private void ButtonSelectFile(object sender, RoutedEventArgs e)
        {
            if (openFileDialog.ShowDialog() == true)
            {
                if (FileObject.fileType.Equals(".xml"))
                {
                    labelFileName.Content = openFileDialog.SafeFileName.Replace(".xml", "");
                }
                else
                {
                    labelFileName.Content = openFileDialog.SafeFileName.Replace(".json", "");
                }
            }

            if (FileObject.fileType.Equals(".xml"))
            {
                LoadXml();
            }
            else
            {
                LoadJson();
            }
        }

        // Загрузка Xml файла
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

                XDocument xmlDoc = XDocument.Load(openFileDialog.FileName);
                var groupedElements = xmlDoc.Root.Elements().GroupBy(element => element.Name.LocalName).Select(group => new
                {
                    ElementName = group.Key,
                    Elements = group.Nodes()
                });

                foreach (var group in groupedElements)
                {
                    ListBox listBox = new ListBox()
                    {
                        Background = Brushes.Transparent,
                        BorderThickness = new Thickness(0)
                    };

                    tagAndElements.Add(group.ElementName, new List<string>());

                    listBox.Items.Add(new ListBoxItem
                    {
                        Content = group.ElementName.ToString(),
                        FontWeight = FontWeights.Bold
                    });
                    foreach (var element in group.Elements)
                    {
                        listBox.Items.Add(new ListBoxItem
                        {
                            Content = element.ToString()
                        });                       

                        tagAndElements[group.ElementName].Add(element.ToString());

                    }                    

                    listBoxItemsList.Items.Add(listBox);
                }

                comboBoxTagDelete.ItemsSource = tagAndElements.Keys;
                comboBoxElementPostIn.ItemsSource = tagAndElements.Keys;
                comboBoxElementDeleteFrom.ItemsSource = tagAndElements.Keys;

                buttonClear.IsEnabled = true;
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

        // Загрузка Json файла
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

                foreach (var item in json)
                {
                    foreach(var ite in item.Value)
                    {
                        foreach(var it in ite)
                        {
                            ListBox listBox = new ListBox()
                            {
                                Background = Brushes.Transparent,
                                BorderThickness = new Thickness(0)
                            };

                            tagAndElements.Add(it.Path.ToString().Split('.')[1], new List<string>());

                            listBox.Items.Add(new ListBoxItem
                            {
                                Content = it.Path.ToString().Split('.')[1],
                                FontWeight = FontWeights.Bold
                            });
                            foreach (var element in ite)
                            {
                                foreach (var elem in element)
                                {
                                    listBox.Items.Add(new ListBoxItem
                                    {
                                        Content = elem.ToString()
                                    });

                                    tagAndElements[it.Path.ToString().Split('.')[1]].Add(elem.ToString());
                                }
                            }

                            listBoxItemsList.Items.Add(listBox);
                        }
                    }
                }

                comboBoxTagDelete.ItemsSource = tagAndElements.Keys;
                comboBoxElementPostIn.ItemsSource = tagAndElements.Keys;
                comboBoxElementDeleteFrom.ItemsSource = tagAndElements.Keys;

                buttonClear.IsEnabled = true;
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
        private void ButtonAcceptChanges(object sender, RoutedEventArgs e)
        {
            try
            {
                // Добавление тега
                if (!string.IsNullOrEmpty(textBoxTagPost.Text))
                {
                    if (FileObject.fileType.Equals(".xml"))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(openFileDialog.FileName);
                        XmlNode rootNode = doc.DocumentElement;

                        XmlNode newTag = doc.CreateNode(XmlNodeType.Element, textBoxTagPost.Text, "");
                        rootNode.AppendChild(newTag);
                        doc.Save(openFileDialog.FileName);

                        textBoxTagPost.Text = null; 
                    }
                    else
                    {
                        JObject jsonObject = JObject.Parse(File.ReadAllText(openFileDialog.FileName));

                        JObject newNode = new JObject();
                        jsonObject["Root"][textBoxTagPost.Text] = newNode;

                        File.WriteAllText(openFileDialog.FileName, jsonObject.ToString());
                    }
                }

                // Удаление тега
                if (comboBoxTagDelete.SelectedItem != null)
                {
                    if (FileObject.fileType.Equals(".xml"))
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
                    if (FileObject.fileType.Equals(".xml"))
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

                        textBoxElementPost.Text = null; 
                    }
                    else
                    {
                        JObject jsonObject = JObject.Parse(File.ReadAllText(openFileDialog.FileName));

                        JObject selectedNodeObject = (JObject)jsonObject["Root"][comboBoxElementPostIn.SelectedItem.ToString()];
                        selectedNodeObject[textBoxElementPost.Text] = "";

                        File.WriteAllText(openFileDialog.FileName, jsonObject.ToString());
                    }
                }

                // Удаление элемента из тега
                if (comboBoxElementDeleteFrom.SelectedItem != null && comboBoxElementDelete.SelectedItem != null)
                {
                    if (FileObject.fileType.Equals(".xml"))
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(openFileDialog.FileName);
                        XmlNode root = doc.DocumentElement;
                        XmlNode parentTag = root.SelectSingleNode(comboBoxElementDeleteFrom.SelectedItem.ToString());

                        if (parentTag != null)
                        {
                            string elementString = comboBoxElementDelete.SelectedItem.ToString();
                            int startIndex = elementString.IndexOf("<") + 1;
                            int endIndex = elementString.IndexOf(">", startIndex);
                            elementString = elementString.Substring(startIndex, endIndex - startIndex);

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
                        string elementString = comboBoxElementDelete.SelectedItem.ToString();
                        int startIndex = elementString.IndexOf("\"") + 1;
                        int endIndex = elementString.IndexOf("\"", startIndex);
                        elementString = elementString.Substring(startIndex, endIndex - startIndex);

                        JObject jsonObject = JObject.Parse(File.ReadAllText(openFileDialog.FileName));

                        JObject parentNode = (JObject)jsonObject["Root"][comboBoxElementDeleteFrom.SelectedItem.ToString()];
                        parentNode.Property(elementString).Remove();

                        File.WriteAllText(openFileDialog.FileName, jsonObject.ToString());
                    }
                }

                // Обновление информации
                if (FileObject.fileType.Equals(".xml"))
                {
                    LoadXml();
                }
                else
                {
                    LoadJson();
                }

                InfoWindow infoWindow = new InfoWindow("Изменения успешно применены!", Brushes.LimeGreen);
                infoWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                InfoWindow infoWindow = new InfoWindow("Не удалось изменить элемент.", Brushes.Firebrick);
                infoWindow.ShowDialog();

                Console.WriteLine(ex.Message);
            }           
        }
        
        // Разблокировка кнопок, если написан контент в TextBox'ах
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            buttonAcceptChanges.IsEnabled = !string.IsNullOrWhiteSpace(textBoxTagPost.Text) || !string.IsNullOrWhiteSpace(textBoxElementPost.Text);
            buttonClear.IsEnabled = !string.IsNullOrWhiteSpace(textBoxTagPost.Text) || !string.IsNullOrWhiteSpace(textBoxElementPost.Text);
        }

        // Редактирование элементов тега - НЕ СДЕЛАНО
        private void ButtonEdit(object sender, RoutedEventArgs e)
        {
            try
            {
                // логика - можно сделать диалоговое окно

                InfoWindow infoWindow = new InfoWindow("Изменения успешно применены!", Brushes.LimeGreen);
                infoWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                InfoWindow infoWindow = new InfoWindow("Не удалось изменить элемент.", Brushes.Firebrick);
                infoWindow.ShowDialog();

                Console.WriteLine(ex.Message);
            }
        }

        // Разблокировка кнопок, если выбран контент в ComboBox'ах
        private void ComboBoxes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            buttonAcceptChanges.IsEnabled = true;
            buttonClear.IsEnabled = true;
        }

        // Отображение подкатегорий элемента из ComboBoxElementDeleteFrom
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
            buttonClear.IsEnabled = true;
        }
    }
}
