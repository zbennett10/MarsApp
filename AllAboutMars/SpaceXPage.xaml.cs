using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;
using HtmlAgilityPack;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AllAboutMars
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SpaceXPage : Page
    {

        //TODO This page
        //webscrape launch manifest spacex page
        //countdown timer til we get to mars
        //Change listbox to show name of article instead of URL
        //Change listbox to display the month the article was posted next to the article name

        public SpaceXPage()
        {
            this.InitializeComponent();
            Document_Parser();
            Weather_Data_Populator();
        }

        public static List<string> newsLinks = new List<string>();

        //populates page controls with Mars weather data
        private async void Weather_Data_Populator()
        {
            RootObject data = await Get_Weather_Data();

            solBlock.Text = data.report.sol.ToString();
            maxTempBlock.Text = data.report.max_temp.ToString() + "°F";
            minTempBlock.Text = data.report.min_temp.ToString() + "°F";
            atmosBlock.Text = data.report.atmo_opacity;
        }

        //SpaceX Blog Scraper
        private async void Document_Parser()
        {
            HttpClient client = new HttpClient();
            var page = await client.GetStringAsync(new Uri("http://www.spacex.com/news"));
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(page);
            var data = doc.DocumentNode.Descendants();
            foreach(var node in data)
            {
                //finds every node on page that is a link containing the appropriate string value
                if (node.Name == "a" && node.InnerHtml == "Read article")
                {
                    newsLinks.Add("http://www.spacex.com/news" + node.Attributes["href"].Value);
                    Test.Items.Add("http://www.spacex.com/news" + node.Attributes["href"].Value);
                    
                }
            }   
        }

        #region SpaceXPage Event Handlers

        private async void Test_Selection_Changed(object sender, SelectionChangedEventArgs e)
        {
            int index = Test.SelectedIndex;
            var uri = new Uri(newsLinks[index]);
            await Windows.System.Launcher.LaunchUriAsync(uri);
        }

        private void homeButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage), null);
        }

        private void spaceXButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SpaceXPage), null);
        }

        private void stationButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NasaStationPage), null);
        }
        #endregion

        #region NASA API JSON-Object Mapper

        private async Task<RootObject> Get_Weather_Data()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(new Uri("http://marsweather.ingenology.com/v1/latest/?format=json"));
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(RootObject));
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (RootObject)serializer.ReadObject(stream);
            return data;
        }

        //mars weather data class structure
        [DataContract]
        public class Report
        {
            [DataMember]
            public string terrestrial_date { get; set; }

            [DataMember]
            public int sol { get; set; }

            [DataMember]
            public double ls { get; set; }

            [DataMember]
            public double min_temp { get; set; }

            [DataMember]
            public double min_temp_fahrenheit { get; set; }

            [DataMember]
            public double max_temp { get; set; }

            [DataMember]
            public double max_temp_fahrenheit { get; set; }

            [DataMember]
            public double pressure { get; set; }

            [DataMember]
            public string pressure_string { get; set; }

            [DataMember]
            public object abs_humidity { get; set; }

            [DataMember]
            public object wind_speed { get; set; }

            [DataMember]
            public string wind_direction { get; set; }

            [DataMember]
            public string atmo_opacity { get; set; }

            [DataMember]
            public string season { get; set; }

            [DataMember]
            public string sunrise { get; set; }

            [DataMember]
            public string sunset { get; set; }
        }

        [DataContract]
        public class RootObject
        {
            [DataMember]
            public Report report { get; set; }
        }
        #endregion
    }
}
