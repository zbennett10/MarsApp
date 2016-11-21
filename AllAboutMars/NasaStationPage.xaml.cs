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



        private void On_Page_Load(object sender, RoutedEventArgs e)
        {
            stationMap.MapServiceToken = Map_Key_Getter();
            stationNames = new List<string>();
            Station_Name_Fetcher();
            Pin_Populator(Point_Creator(Position_Creator(Station_Location_Fetcher())));
            List<MapIcon> iconList = new List<MapIcon>();

            for (int i = 0; i < stationMap.MapElements.Count; i++)
            {
                if (stationMap.MapElements[i] is MapIcon)
                {
                    iconList.Add((MapIcon)stationMap.MapElements[i]);
                }
            }
            for (int i = 0; i < iconList.Count; i++)
            {
                iconList[i].Title = stationNames[i];
            }
           
        }



        public List<BasicGeoposition> Position_Creator(List<Location> list)
        {
            List<BasicGeoposition> positionList = new List<BasicGeoposition>();
            foreach(Location location in list)
            {
                positionList.Add(new BasicGeoposition { Latitude = Double.Parse(location.Latitude), Longitude = Double.Parse(location.Longitude) });
            }
            return positionList;
        }

        public List<Geopoint> Point_Creator(List<BasicGeoposition> positionList)
        {
            List<Geopoint> pointList = new List<Geopoint>();
            foreach(BasicGeoposition position in positionList)
            {
                pointList.Add(new Geopoint(position));
            }
            return pointList;
        }

        

        public void Pin_Populator(List<Geopoint> points)
        {   
            foreach(Geopoint point in points)
            {
                stationMap.MapElements.Add(new MapIcon { Location = point});
                
            }



            
        }


        public string Map_Key_Getter()
        {
            ResourceLoader resource = new ResourceLoader("Resources");
            string bingKey = resource.GetString("mapToken");
            return bingKey;
        }

        //[XmlRoot(ElementName = "Location", Namespace = "http://sscweb.gsfc.nasa.gov/schema")]
        public class Location
        {
            //    //[XmlElement(ElementName = "Latitude", Namespace = "http://sscweb.gsfc.nasa.gov/schema")]
            public string Latitude { get; set; }
            //    //[XmlElement(ElementName = "Longitude", Namespace = "http://sscweb.gsfc.nasa.gov/schema")]
            public string Longitude { get; set; }
        }

        //[XmlRoot(ElementName = "GroundStation", Namespace = "http://sscweb.gsfc.nasa.gov/schema")]
        public class GroundStation
        {
            //[XmlElement(ElementName = "Id", Namespace = "http://sscweb.gsfc.nasa.gov/schema")]
            public string Id { get; set; }
            //[XmlElement(ElementName = "Name", Namespace = "http://sscweb.gsfc.nasa.gov/schema")]
            public string Name { get; set; }
            //[XmlElement(ElementName = "Location", Namespace = "http://sscweb.gsfc.nasa.gov/schema")]
            //public Location Location { get; set; }
            
        }

        
        public List<Location> Station_Location_Fetcher()
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

        public static List<string> stationNames;
    
        public void Station_Name_Fetcher()
        {

            XDocument loadedData = XDocument.Load("GroundStationInfo.xml");
            var ns = loadedData.Root.GetDefaultNamespace();

            var nameData = from station in loadedData.Descendants(ns + "GroundStation")
                               select new GroundStation
                               {
                                   Name = station.Element(ns + "Name").Value
                               };

            foreach(GroundStation station in nameData)
            {
                stationNames.Add(station.Name);
            }
           
        }

        //private void Station_Name_Fetcher()
        //{
        //    XDocument loadedData = XDocument.Load("GroundStationInfo.xml");
        //    var ns = loadedData.Root.GetDefaultNamespace();
        //    var stationData = from station in loadedData.Descendants(ns + "GroundStation")
        //                   select new GroundStation
        //                   {
        //                       Name = station.Element("Name").Value
        //                   };
        //    List<GroundStation> list = stationData.ToList();
        //    foreach(GroundStation station in list)
        //    {
        //        stationList.Add(station);
        //    }
        //}

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
