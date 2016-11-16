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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AllAboutMars
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static ListBox twitterTest = new ListBox();

        public MainPage()
        {
            this.InitializeComponent();
            twitterStack.Children.Add(twitterTest);
        }

        private static SingleUserAuthorizer authorizer = new SingleUserAuthorizer
        {
            CredentialStore = new SingleUserInMemoryCredentialStore
            {    
            }
        };

        private void OnMainPage_Load(object sender, RoutedEventArgs e)
        {
            var resources = new Windows.ApplicationModel.Resources.ResourceLoader("Resources");
            authorizer.CredentialStore.ConsumerKey = resources.GetString("consumerToken");
            authorizer.CredentialStore.ConsumerSecret = resources.GetString("consumerSecret");
            authorizer.CredentialStore.OAuthToken = resources.GetString("accessToken");
            authorizer.CredentialStore.OAuthTokenSecret = resources.GetString("accessSecret");

            var results = SearchTwitter("Mars Nasa SpaceX");
            twitterTest.Items.Clear();
            results.ForEach(tweet => twitterTest.Items.Add(tweet.User.Name + ":" + tweet.Text));

        }

        private static List<Status> SearchTwitter(string searchTerm)
        {
            var twitterContext2 = new TwitterContext(authorizer);

            var srch =
               Enumerable.SingleOrDefault((from search in twitterContext2.Search
                                           where search.Type == SearchType.Search &&
                                           search.Query == searchTerm &&
                                           search.Count == 10
                                           select search));
            if (srch != null && srch.Statuses.Count > 0)
            {
                return srch.Statuses.ToList();
            }

            return new List<Status>();
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

        private async void testNasaButton_Click(object sender, RoutedEventArgs e)
        {
            //goes up to at least photos[500]
            NasaProxy.RootObject data = await NasaProxy.GetNasaData();
            BitmapImage image = new BitmapImage(new Uri(data.photos[5].img_src));

            testImage.Source = image;
            //nasaTest.Text = data.photos[0].camera.full_name;
            nasaTest.Text = data.photos[5].rover.name;

        }

    }
}
