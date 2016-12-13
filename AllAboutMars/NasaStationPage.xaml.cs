using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Xml.Linq;
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

            stationMap.MapServiceToken = Map_Key_Fetcher();
            Pin_Populator(Point_Creator(Position_Creator(Station_Location_Fetcher())));
            MapIcon_Name_Populator(MapIcon_Populator(), Station_Name_Fetcher());
        }

        //fetches bing map key
        private string Map_Key_Fetcher()
        {
            ResourceLoader resource = new ResourceLoader("Resources");
            string bingKey = resource.GetString("mapToken");
            return bingKey;
        }

        //TODO
        //add flag icons instead of pins
        //display station name/location on pin click

        #region Map Control Functionality

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
        #endregion

        #region XML Document-Object Map/Data Functionality
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
            //work on switching this over to XmlReader
           
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
    
        //fetches every station name from xml document
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
        #endregion

        #region NasaStationPage Control Event Handlers
        //navigation buttons
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
    }
}
