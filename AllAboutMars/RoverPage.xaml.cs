using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
            NasaProxy.RootObject curiosityData = await NasaProxy.GetNasaData("curiosity", 1514);
            NasaProxy.RootObject opportunityData = await NasaProxy.GetNasaData("opportunity", 2207);
            curiosityStatus.Text = curiosityData.photos[0].rover.status;
            opportunityStatus.Text = opportunityData.photos[0].rover.status;
        }

        private async void Curiosity_Image_Populator()
        {
            NasaProxy.RootObject curiosityData = await NasaProxy.GetNasaData("curiosity", 1514);
            BitmapImage image = new BitmapImage(new Uri(curiosityData.photos[0].img_src));
            curiosityImage.Source = image;
        }

       private async void Opportunity_Image_Populator()
        {
            NasaProxy.RootObject opportunityData = await NasaProxy.GetNasaData("opportunity", 2207);
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
    }
}
