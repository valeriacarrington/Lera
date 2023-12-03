using System.Xml;
using Microsoft.Maui.Storage;
using System;
using System.IO;
using System.Xml.Xsl;
using Microsoft.Maui.Controls;


namespace Lera
{
    public partial class MainPage : ContentPage
    {
        private Button xmlFileButton;
        private Button xslFileButton;
        private Button exitButton;
        private Button SearchButton;
        private Button SearchByAttributesButton;
        private Button xmlToHtmlButton;
        XmlDocument xmlFile;
        XslCompiledTransform xslFile;
        private string selectedAttribute = "";
        private string keywords = "";
        private Picker attributePicker;
        private Entry searchEntry;
        private Picker analysisMethodPicker;
        private XMLAnalyzer xmlAnalyzer;
        private Button clearButton;
        System.Collections.ObjectModel.ObservableCollection<string> newItems = new System.Collections.ObjectModel.ObservableCollection<string>();
        public MainPage()
        {
            xmlFileButton = new Button
            {
                Text = "Вибрати файл XML"
            };
            xmlFileButton.Clicked +=OnXmlFileButtonClicked;

            xslFileButton = new Button
            {
                Text = "Вибрати файл XSL"
            };
            xslFileButton.Clicked += OnXslFileButtonClicked;

            exitButton = new Button
            {
                Text = "Вихід"
            };
            exitButton.Clicked += OnExitButtonClicked;

            xmlToHtmlButton = new Button
            {
                Text = "Трансформувати XML"
            };
            xmlToHtmlButton.Clicked += OnxmlToHtmlButtonClicked;
            StackLayout stackLayout = new StackLayout();
            attributePicker = new Picker
            {
                Title = "Оберіть атрибут"
            };
            attributePicker.ItemsSource = newItems;
            searchEntry = new Entry
            {
                Placeholder = "Введіть ключове слово"
            };
            var saveButton = new Button
            {
                Text = "Зберегти"
            };
            SearchButton = new Button
            {
                Text = "Пошук по ключовим словам"
            };
            SearchButton.Clicked += OnSearchButtonClicked;
            clearButton = new Button
            {
                Text = "Очистити"
            };
            clearButton.Clicked += OnClearButtonClicked;
            SearchByAttributesButton = new Button
            {
                Text = "Пошук по атрибутам"
            };
            SearchByAttributesButton.Clicked += OnSearchByAttributesButtonClicked;
            saveButton.Clicked += OnSaveButtonClicked;
            analysisMethodPicker = new Picker
            {
                Title = "Виберіть метод аналізу XML"
            };
            analysisMethodPicker.Items.Add("SAX API");
            analysisMethodPicker.Items.Add("DOM API");
            analysisMethodPicker.Items.Add("LINQ to XML");
            stackLayout.Children.Add(xmlFileButton);
            stackLayout.Children.Add(xslFileButton);
            stackLayout.Children.Add(analysisMethodPicker);
            stackLayout.Children.Add(attributePicker);
            stackLayout.Children.Add(searchEntry);
            stackLayout.Children.Add(saveButton);
            stackLayout.Children.Add(SearchButton);
            stackLayout.Children.Add(SearchByAttributesButton);
            stackLayout.Children.Add(xmlToHtmlButton);
            stackLayout.Children.Add(clearButton);
            stackLayout.Children.Add(exitButton);
            analysisMethodPicker.SelectedIndexChanged += OnAnalysisMethodSelected;
            Content = stackLayout;
        }

        private void OnClearButtonClicked(object sender, EventArgs e)
        {
            selectedAttribute = "";
            keywords = "";
            searchEntry.Text = "";
            xmlFile = null;
            xslFile = null;
            attributePicker.ItemsSource = new System.Collections.ObjectModel.ObservableCollection<string>();
            analysisMethodPicker.SelectedItem = "";
        }

        private void OnSearchByAttributesButtonClicked(object sender, EventArgs e)
        {
            var listXElements = xmlAnalyzer.SearchElementsByAttribute(xmlFile, selectedAttribute);
            if (listXElements != null)
            {
                var result = "Count elements that contain a attributes:" + listXElements.Count() + "\n";
                foreach (var item in listXElements)
                {
                    result += item.ToString() + "\n\n";
                }
                DisplayAlert("Result", result, "OK");
            }
        }

        private void OnSearchButtonClicked(object sender, EventArgs e)
        {
            var listXElements = xmlAnalyzer.SearchElementsByKeywords(xmlFile, keywords);
            if (listXElements != null)
            {
                var result = "Count elements that contain a keyword:" + listXElements.Count()+"\n";
                foreach (var item in listXElements)
                {
                    result += item.ToString() + "\n\n";
                }
                DisplayAlert("Result", result, "OK");
            }
        }

        private async void OnXmlFileButtonClicked(object sender, EventArgs e)
        {
            var file = await FilePicker.PickAsync();

            if (file != null)
            {
                string filePath = file.FullPath;
                using (var stream = new FileStream(filePath, FileMode.Open))
                using (var reader = new StreamReader(stream))
                {
                    string xmlContent = await reader.ReadToEndAsync();
                    xmlFile = new XmlDocument();
                    xmlFile.LoadXml(xmlContent);
                }
            }
        }

        private void OnSaveButtonClicked(object sender, EventArgs e)
        {
            // Зберігання вибраного атрибута і ключових слів у змінні
            selectedAttribute = attributePicker.SelectedItem.ToString();
            keywords = searchEntry.Text;

            // Додаткові дії, які можна виконати з отриманими значеннями
            // Наприклад, вивід у сповіщенні
            DisplayAlert("Збережено", $"Атрибут: {selectedAttribute}, Ключові слова: {keywords}", "OK");
        }
        private async void OnXslFileButtonClicked(object sender, EventArgs e)
        {
            var file = await FilePicker.PickAsync();

            if (file != null)
            {
                string filePath = file.FullPath;
                using (var stream = new FileStream(filePath, FileMode.Open))
                using (var reader = new StreamReader(stream))
                {
                    string xslContent = await reader.ReadToEndAsync();
                    xslFile = new XslCompiledTransform();
                    xslFile.Load(XmlReader.Create(new StringReader(xslContent)));
                }
            }
        }
        private void OnAnalysisMethodSelected(object sender, EventArgs e)
        {
            // Виберіть метод аналізу XML в залежності від вибору користувача
            string selectedMethod = analysisMethodPicker.SelectedItem.ToString();

            switch (selectedMethod)
            {
                case "SAX API":
                    xmlAnalyzer=new XMLAnalyzer( new SAXAnalyzerStrategy());
                    break;
                case "DOM API":
                    xmlAnalyzer = new XMLAnalyzer(new DOMAnalyzerStrategy());
                    break;
                case "LINQ to XML":
                    xmlAnalyzer = new XMLAnalyzer(new LINQtoXMLStrategy());
                    break;
            }
            newItems = new System.Collections.ObjectModel.ObservableCollection<string>(xmlAnalyzer.SearchAttributes(xmlFile));
            attributePicker.ItemsSource = null;
            attributePicker.ItemsSource = newItems;


        }
        private async void OnExitButtonClicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert("Підтвердження", "Чи дійсно ви хочете завершити роботу з програмою?", "Так", "Ні");
            if (result)
            {
                Environment.Exit(0);
            }
        }
        private void OnxmlToHtmlButtonClicked(object sender, EventArgs e)
        {
            // Здійснення трансформації XML у HTML
            string transformedHtml = TransformXmlWithXsl(xmlFile, xslFile);
            SaveHtmlToDesktop(transformedHtml, "output.html");
        }
        private string TransformXmlWithXsl(XmlDocument xmlDocument, XslCompiledTransform xslTransform)
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter))
                {
                    xslTransform.Transform(xmlDocument, null, xmlWriter);
                }
                return stringWriter.ToString();
            }
        }
        private void SaveHtmlToDesktop(string htmlContent, string fileName)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string filePath = Path.Combine(desktopPath, fileName);

            File.WriteAllText(filePath, htmlContent);
            Console.WriteLine($"HTML file saved to {filePath}");
        }
    }
}