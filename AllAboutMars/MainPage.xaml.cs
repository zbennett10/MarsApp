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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AllAboutMars
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

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

        public List<Match> twitterLinks = new List<Match>();

        private void On_Page_Load(object sender, RoutedEventArgs e)
        {
            Authorizer_Key_Populator();        
            Link_List_Populator(Mars_Link_Fetcher(), Nasa_Link_Fetcher());
            twitterTest.Items.Clear();
            Search_Result_Mars_Fetcher().ForEach(tweet => twitterTest.Items.Add(tweet.User.Name + ":" + tweet.Text));
            Search_Result_Nasa_Fetcher().ForEach(tweet => twitterTest.Items.Add(tweet.User.Name + ":" + tweet.Text)); 
        }

       private List<Status> Search_Result_Mars_Fetcher()
        {
            return SearchTwitter("Mars SpaceX");
        }

        private List<Status> Search_Result_Nasa_Fetcher()
        {
            return SearchTwitter("Nasa Mars");
        }

        private List<Match> Mars_Link_Fetcher()
        {
            return Link_Finder(Search_Result_Mars_Fetcher());
        }

        private List<Match> Nasa_Link_Fetcher()
        {
            return Link_Finder(Search_Result_Nasa_Fetcher());
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

        private void Link_List_Populator(List<Match> list1, List<Match> list2)
        {
            list1.ForEach(link => twitterLinks.Add(link));
            list2.ForEach(link => twitterLinks.Add(link));
        }

        private static List<Status> SearchTwitter(string searchTerm)
        {
            var twitterContext = new TwitterContext(authorizer);

            var srch =
               Enumerable.SingleOrDefault((from search in twitterContext.Search
                                           where search.Type == SearchType.Search &&
                                           search.Query == searchTerm &&
                                           search.ResultType ==  ResultType.Popular &&
                                           search.Count == 7
                                           select search));
            if (srch != null && srch.Statuses.Count > 0)
            {
                return srch.Statuses.ToList();
            }
            return new List<Status>();
        }

        private async void twitterTest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = twitterTest.SelectedIndex;
            string uriToLaunch = twitterLinks[selectedIndex].ToString();
            var uri = new Uri(uriToLaunch);
            await  Windows.System.Launcher.LaunchUriAsync(uri);
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
