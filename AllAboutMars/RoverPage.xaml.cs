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
    public sealed partial class RoverPage : Page
    {
        //TODO This page
        //Create a view that is populated by curiosity rover images.
        //implement user-selected filters to view

        public RoverPage()
        {
            this.InitializeComponent();
        }

        //curiosity max sol - 1514
        //spirit max sol - 2207
        //opportunity max sol - 4546

        private void On_Page_Load(object sender, RoutedEventArgs e)
        {
            Status_Populator();
            Curiosity_Image_Populator();
            Opportunity_Image_Populator();
        }

        private async void Status_Populator()
        {
            RootObject curiosityData = await GetNasaData("curiosity", 1514);
            RootObject opportunityData = await GetNasaData("opportunity", 2207);
            curiosityStatus.Text = curiosityData.photos[0].rover.status;
            opportunityStatus.Text = opportunityData.photos[0].rover.status;
        }

        private async void Curiosity_Image_Populator()
        {
            RootObject curiosityData = await GetNasaData("curiosity", 1514);
            BitmapImage image = new BitmapImage(new Uri(curiosityData.photos[0].img_src));
            curiosityImage.Source = image;
        }

       private async void Opportunity_Image_Populator()
        {
            RootObject opportunityData = await GetNasaData("opportunity", 2207);
            BitmapImage image = new BitmapImage(new Uri(opportunityData.photos[0].img_src));
            opportunityImage.Source = image;
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

        public async static Task<RootObject> GetNasaData(string rover, int maxSol)
        {
            var resources = new Windows.ApplicationModel.Resources.ResourceLoader("Resources");
            var token = resources.GetString("nasaToken");

            HttpClient http = new HttpClient();
            var response = await http.GetAsync(String.Format("https://api.nasa.gov/mars-photos/api/v1/rovers/{0}/photos?sol={1}&api_key={2}", rover, maxSol, token));
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(RootObject));
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (RootObject)serializer.ReadObject(memoryStream);

            return data;
        }

        [DataContract]
        public class Camera
        {
            [DataMember]
            public int id { get; set; }

            [DataMember]
            public string name { get; set; }

            [DataMember]
            public int rover_id { get; set; }

            [DataMember]
            public string full_name { get; set; }
        }

        [DataContract]
        public class Camera2
        {
            [DataMember]
            public string name { get; set; }

            [DataMember]
            public string full_name { get; set; }
        }

        [DataContract]
        public class Rover
        {
            [DataMember]
            public int id { get; set; }

            [DataMember]
            public string name { get; set; }

            [DataMember]
            public string landing_date { get; set; }

            [DataMember]
            public string launch_date { get; set; }

            [DataMember]
            public string status { get; set; }

            [DataMember]
            public int max_sol { get; set; }

            [DataMember]
            public string max_date { get; set; }

            [DataMember]
            public int total_photos { get; set; }

            [DataMember]
            public List<Camera2> cameras { get; set; }
        }

        [DataContract]
        public class Photo
        {
            [DataMember]
            public int id { get; set; }

            [DataMember]
            public int sol { get; set; }

            [DataMember]
            public Camera camera { get; set; }

            [DataMember]
            public string img_src { get; set; }

            [DataMember]
            public string earth_date { get; set; }

            [DataMember]
            public Rover rover { get; set; }
        }

        [DataContract]
        public class RootObject
        {
            [DataMember]
            public List<Photo> photos { get; set; }
        }
    }
}
