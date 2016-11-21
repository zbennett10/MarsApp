using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Xml;
using Windows.Web.Http;
using System.Net;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AllAboutMars
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NasaStationPage : Page
    {
        public NasaStationPage()
        {
            this.InitializeComponent();
            
        }

        private void On_Page_Load(object sender, RoutedEventArgs e)
        {
            stationMap.MapServiceToken = Map_Key_Getter();
        }

        public string Map_Key_Getter()
        {
            ResourceLoader resource = new ResourceLoader("Resources");
            string bingKey = resource.GetString("mapToken");
            return bingKey;
        }

        //beginning of nasa ssc api call
        //http://sscweb.sci.gsfc.nasa.gov/WS/sscr/2
        public async void StationFetcher()
        {
            //XmlDocument doc = new XmlDocument();
            //doc.Load(http://sscweb.sci.gsfc.nasa.gov/WS/sscr/2/groundStations);
            //XmlElement root = doc.DocumentElement;
           // HttpClient http = new HttpClient();
            //var response = await http.GetAsync(new Uri("http://sscweb.sci.gsfc.nasa.gov/WS/sscr/2/groundStations"));
            XmlDocument doc = new XmlDocument();
            HttpWebRequest request = WebRequest.Create("http://sscweb.sci.gsfc.nasa.gov/WS/sscr/2/groundStations") as HttpWebRequest;
            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
            doc.Load(response.GetResponseStream());
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(doc.NameTable);
            namespaceManager.AddNamespace("rest", "http://sscweb.gsfc.nasa.gov/schema");

            XmlNodeList nodes = doc.

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
    }
}
