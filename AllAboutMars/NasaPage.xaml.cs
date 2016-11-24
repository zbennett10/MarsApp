using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AllAboutMars
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NasaPage : Page
    {
        //TODO this page
        //edit look and feel of this page


        public NasaPage()
        {
            this.InitializeComponent();
        }

        private async void On_Page_Load(object sender, RoutedEventArgs e)
        {
            RootObject data = await GetAPOD(Nasa_Key_Getter());
            apodImage.Source = Image_Creator(data.hdurl);
            descTextBlock.Text = data.explanation;
        }

        private BitmapImage Image_Creator(string url)
        {
            BitmapImage image = new BitmapImage(new Uri(url));
            return image;
        }

        private string Nasa_Key_Getter()
        {
            ResourceLoader resource = new ResourceLoader("Resources");
            return resource.GetString("nasaToken");  
        }

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), null);
        }

        private void nasaButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NasaPage), null);
        }

        private void spaceXButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SpaceXPage), null);
        }

        private void roversButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RoverPage), null);
        }

        private void stationButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NasaStationPage), null);
        }

        public static async Task<RootObject> GetAPOD(string key)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(String.Format("https://api.nasa.gov/planetary/apod?api_key={0}", key));
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(RootObject));
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (RootObject)serializer.ReadObject(memoryStream);
            return data;
        }

        [DataContract]
        public class RootObject
        {
            [DataMember]
            public string copyright { get; set; }

            [DataMember]
            public string date { get; set; }

            [DataMember]
            public string explanation { get; set; }

            [DataMember]
            public string hdurl { get; set; }

            [DataMember]
            public string media_type { get; set; }

            [DataMember]
            public string service_version { get; set; }

            [DataMember]
            public string title { get; set; }

            [DataMember]
            public string url { get; set; }
        }
    }
}
