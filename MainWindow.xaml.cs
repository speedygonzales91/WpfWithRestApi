using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfWithRestApi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void uploadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Filter =  "Image files (*.png; *.jpg)|*.png;*.jpg;*.jpeg| All Files (*.*)|*.*";
            if (fileDialog.ShowDialog() == true)
            {
                string filename = fileDialog.FileName;

                selectedImage.Source = new BitmapImage(new Uri(filename));

                IdentifyImageAsync(filename);
            }
        }

        private async void IdentifyImageAsync(string filename)
        {
            var file = File.ReadAllBytes(filename);

            string url = "https://southcentralus.api.cognitive.microsoft.com/customvision/v3.0/Prediction/546b82b7-a125-47c7-8270-243f841b4bb4/classify/iterations/Iteration1/image";
            string predictionKey = "ac088cbe67dc4946baf2eb394e1675d1";
            string contentType = "application/octet-stream";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Prediction-Key", predictionKey);


                using (var content = new ByteArrayContent(file))
                {

                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

                    var response = await client.PostAsync(url, content);

                    var responseString = await response.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
