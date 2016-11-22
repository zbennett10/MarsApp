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
using System.Xml.Linq;
using System.Xml.Serialization;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;

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

        //TODO
        //pull recent text from http://www.planet4589.org/space/jsr/jsr.html (data about recent space happening/launches around world)

        //http://www.celestrak.com/NORAD/documentation/tle-fmt.asp norad two-line element set format pulling data from satellites 

        //String.Format("http://ws.geonames.org/countryCode?lat={0}&lng={1}&username=zbennett10",lat, lon)

        //figure out how to use geometry and trajectory urls for /spaseObservatories call to SSC


        //populates map control on page load
        private void On_Page_Load(object sender, RoutedEventArgs e)
        {
            stationMap.MapServiceToken = Map_Key_Getter();
            Pin_Populator(Point_Creator(Position_Creator(Station_Location_Fetcher())));
            MapIcon_Name_Populator(MapIcon_Populator(), Station_Name_Fetcher());
        }

      //gets list of every map element that is a map icon;
       private List<MapIcon> MapIcon_Populator()
        {
            List<MapIcon> icons = new List<MapIcon>();
            for(int i = 0; i < stationMap.MapElements.Count; i++)
            {
                if (stationMap.MapElements[i] is MapIcon) icons.Add((MapIcon)stationMap.MapElements[i]);
            }
            return icons;
        }

        //sets every map icon's title prop in sequential order with its corresponding station name
        private void MapIcon_Name_Populator(List<MapIcon> icons, List<string> names)
        {
            for (int i = 0; i < icons.Count; i++)
            {
                icons[i].Title = names[i];
            }
        }


        //converts latitude and longitude of ground station to geoposition
        private List<BasicGeoposition> Position_Creator(List<Location> list)
        {
            List<BasicGeoposition> positionList = new List<BasicGeoposition>();
            foreach(Location location in list)
            {
                positionList.Add(new BasicGeoposition { Latitude = Double.Parse(location.Latitude), Longitude = Double.Parse(location.Longitude) });
            }
            return positionList;
        }

        //converts geoposition to point on bing map
        private List<Geopoint> Point_Creator(List<BasicGeoposition> positionList)
        {
            List<Geopoint> pointList = new List<Geopoint>();
            foreach(BasicGeoposition position in positionList)
            {
                pointList.Add(new Geopoint(position));
            }
            return pointList;
        }

        
        //populates map with point markers
        private void Pin_Populator(List<Geopoint> points)
        {   
            foreach(Geopoint point in points)
            {
                stationMap.MapElements.Add(new MapIcon { Location = point});    
            } 
        }

        //fetches bing map key
        private string Map_Key_Getter()
        {
            ResourceLoader resource = new ResourceLoader("Resources");
            string bingKey = resource.GetString("mapToken");
            return bingKey;
        }

       
        //Data structures to hold ground station data
        private class Location
        { 
            public string Latitude { get; set; }
            public string Longitude { get; set; }
        }

        private class GroundStation
        {
            public string Id { get; set; }
            public string Name { get; set; }  
        }

        //fetches latitude and longitude data of every station in xml document
        private List<Location> Station_Location_Fetcher()
        {
            XDocument loadedData = XDocument.Load("GroundStationInfo.xml");
            var ns = loadedData.Root.GetDefaultNamespace();

            var locationData = from location in loadedData.Descendants(ns + "Location")
                              select new Location
                              {
                                  
                                  Latitude = location.Element(ns + "Latitude").Value,
                                  Longitude = location.Element(ns + "Longitude").Value
                              };
            return locationData.ToList();
        }

        //global variable that hold station name data
        //public static List<string> stationNames;
    
        //fetches every station name from xml document
        //populates stationNames global list
        private List<string> Station_Name_Fetcher()
        {

            XDocument loadedData = XDocument.Load("GroundStationInfo.xml");
            var ns = loadedData.Root.GetDefaultNamespace();

            var nameData = from station in loadedData.Descendants(ns + "GroundStation")
                               select new GroundStation
                               {
                                   Name = station.Element(ns + "Name").Value
                               };
            List<string> list = new List<string>();
            foreach(GroundStation station in nameData)
            {
                list.Add(station.Name);
            }
            return list;
        }

        //navigation buttons
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
