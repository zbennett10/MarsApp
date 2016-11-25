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
using LinqToTwitter;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading.Tasks;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AllAboutMars
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //TODO
        //try to implement xmlreader instead of xdocument
        //display username above scrollviwer and edit out http part of tweet so that only message shows

        public MainPage()
        {
            this.InitializeComponent();
        }

        private static SingleUserAuthorizer authorizer = new SingleUserAuthorizer
        {
            CredentialStore = new SingleUserInMemoryCredentialStore
            {    
            }
        };

        public List<Match> spaceXLinks = new List<Match>();
        public List<Match> nasaLinks = new List<Match>();

        private void On_Page_Load(object sender, RoutedEventArgs e)
        {
            Authorizer_Key_Populator();        
            Link_List_Populator(SpaceX_Link_Fetcher(), Nasa_Link_Fetcher());
            spaceXLB.Items.Clear();
            Timeline_Fetcher("SpaceX").ForEach(tweet => spaceXLB.Items.Add(tweet.User.Name + ":" + tweet.Text));
            Timeline_Fetcher("Nasa").ForEach(tweet => nasaLB.Items.Add(tweet.User.Name + ":" + tweet.Text)); 
        }

        private List<Match> SpaceX_Link_Fetcher()
        {
            return Link_Finder(Timeline_Fetcher("SpaceX"));
        }

        private List<Match> Nasa_Link_Fetcher()
        {
            return Link_Finder(Timeline_Fetcher("Nasa"));
        }
        
       private void Authorizer_Key_Populator()
        {
            var resources = new Windows.ApplicationModel.Resources.ResourceLoader("Resources");
            authorizer.CredentialStore.ConsumerKey = resources.GetString("consumerToken");
            authorizer.CredentialStore.ConsumerSecret = resources.GetString("consumerSecret");
            authorizer.CredentialStore.OAuthToken = resources.GetString("accessToken");
            authorizer.CredentialStore.OAuthTokenSecret = resources.GetString("accessSecret");
        }

        private static List<Match> Link_Finder(List<Status> tweetList)
        {
           string pattern = @"\b(?:https?:)\S+\b";
            List<Match> matches = new List<Match>();
            tweetList.ForEach(tweet => matches.Add(Regex.Match(tweet.Text, pattern)));
            return matches;     
       }

        private void Link_List_Populator(List<Match> spaceXMatches, List<Match> nasaMatches)
        {
            spaceXMatches.ForEach(link => spaceXLinks.Add(link));
            nasaMatches.ForEach(link => nasaLinks.Add(link));
        }


        private  static List<Status> Timeline_Fetcher(string user)
        {
            using (var twitterContext = new TwitterContext(authorizer))
            {
                var tweets = (from tweet in twitterContext.Status
                              where tweet.Type == StatusType.User
                              && tweet.ScreenName == user
                              && tweet.Count == 10                            
                              select tweet).ToList();
                return tweets;
            }
           
        }

        private async void spaceXLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {        
            var uri = new Uri(spaceXLinks[spaceXLB.SelectedIndex].ToString());
            await  Windows.System.Launcher.LaunchUriAsync(uri);
        }

        private async void nasaLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var uri = new Uri(nasaLinks[nasaLB.SelectedIndex].ToString());
            await Windows.System.Launcher.LaunchUriAsync(uri);
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
