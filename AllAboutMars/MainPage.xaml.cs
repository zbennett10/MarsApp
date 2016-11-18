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

        private void OnMainPage_Load(object sender, RoutedEventArgs e)
        {
            var resources = new Windows.ApplicationModel.Resources.ResourceLoader("Resources");
            authorizer.CredentialStore.ConsumerKey = resources.GetString("consumerToken");
            authorizer.CredentialStore.ConsumerSecret = resources.GetString("consumerSecret");
            authorizer.CredentialStore.OAuthToken = resources.GetString("accessToken");
            authorizer.CredentialStore.OAuthTokenSecret = resources.GetString("accessSecret");
   
            var resultsMars = SearchTwitter("Mars SpaceX");
            var resultsNasa = SearchTwitter("Nasa Mars");

            var marsLinks = Link_Finder(resultsMars);
            var nasaLinks = Link_Finder(resultsNasa);

            Link_List_Populator(marsLinks, nasaLinks);

            twitterTest.Items.Clear();
            resultsMars.ForEach(tweet => twitterTest.Items.Add(tweet.User.Name + ":" + tweet.Text));
            resultsNasa.ForEach(tweet => twitterTest.Items.Add(tweet.User.Name + ":" + tweet.Text));
            
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
                                           search.Count == 5
                                           select search));
            if (srch != null && srch.Statuses.Count > 0)
            {
                return srch.Statuses.ToList();
            }
            return new List<Status>();
        }
  
        private void twitterSearchButton_Click(object sender, RoutedEventArgs e)
        {
           var searchResult =  SearchTwitter(twitterSearchTextBox.Text);
            twitterTest.Items.Clear();
            foreach (var tweet in searchResult)
            {
                twitterTest.Items.Add(tweet.User.Name + ":" + tweet.Text);
            }
        }

        private async void twitterTest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ListBoxItem lbi = ((sender as ListBox).SelectedItem as ListBoxItem);
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





        //private List<Status> list;

        //private void GetMostRecent10HomeTimeLine()
        //{
        //    var twitterContext = new TwitterContext(authorizer);


        //    var tweets = from tweet in twitterContext.Status
        //                 where tweet.Type == StatusType.Home &&
        //                 tweet.Count == 5
        //                 select tweet;

        //    list = tweets.ToList();
        //}

        //private async void testNasaButton_Click(object sender, RoutedEventArgs e)
        //{
        //    //goes up to at least photos[500]
        //    NasaProxy.RootObject data = await NasaProxy.GetNasaData();
        //    BitmapImage image = new BitmapImage(new Uri(data.photos[5].img_src));

        //    testImage.Source = image;
        //    //nasaTest.Text = data.photos[0].camera.full_name;
        //    //nasaTest.Text = data.photos[5].rover.name;

        //}

    }
}
