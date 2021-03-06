﻿using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using LinqToTwitter;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

//[assembly: InternalsVisibleTo("MainTest.Test")]
namespace AllAboutMars
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
 
    public sealed partial class MainPage : Page
    {
        //TODO ----
        // Implement twitter media viewer that pops up in a window

        public MainPage()
        {
            this.InitializeComponent();
        }

        #region Twitter API Functionality 
        //twitter API functionality

        //instantiates user authentication for use with Twitter API
        private static SingleUserAuthorizer authorizer = new SingleUserAuthorizer
        {
            CredentialStore = new SingleUserInMemoryCredentialStore
            {    
            }
        };

        private void Authorizer_Key_Populator()
        {
            var resources = new Windows.ApplicationModel.Resources.ResourceLoader("Resources");
            authorizer.CredentialStore.ConsumerKey = resources.GetString("consumerToken");
            authorizer.CredentialStore.ConsumerSecret = resources.GetString("consumerSecret");
            authorizer.CredentialStore.OAuthToken = resources.GetString("accessToken");
            authorizer.CredentialStore.OAuthTokenSecret = resources.GetString("accessSecret");
        }

        private static List<Status> Timeline_Fetcher(string user)
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
        #endregion

        #region Tweet Link Scraping Functionality
        public static List<Match> spaceXLinks = new List<Match>();
        public static List<Match> nasaLinks = new List<Match>();


        private List<Match> SpaceX_Link_Fetcher()
        {
            return Link_Finder(Timeline_Fetcher("SpaceX"));
        }

        private List<Match> Nasa_Link_Fetcher()
        {
            return Link_Finder(Timeline_Fetcher("Nasa"));
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
        #endregion

        #region MainPage Event Handlers

        public void On_Page_Load(object sender, RoutedEventArgs e)
        {
            Authorizer_Key_Populator();
            Link_List_Populator(SpaceX_Link_Fetcher(), Nasa_Link_Fetcher());
            spaceXLB.Items.Clear();

            //takes hyperlinks out of tweet boxes
            Timeline_Fetcher("SpaceX").ForEach(tweet => spaceXLB.Items.Add((new Regex(@"\b(?:https?:)\S+\b")).Replace(tweet.Text, "")));
            Timeline_Fetcher("Nasa").ForEach(tweet => nasaLB.Items.Add((new Regex(@"\b(?:https?:)\S+\b")).Replace(tweet.Text, "")));
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

        //image event handlers --------
        private async void spaceXImage_On_Tapped(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://twitter.com/spaceX"));
        }

        private void spaceXImage_Pointer_Entered(object sender, RoutedEventArgs e)
        {
            spaceXImage.Opacity = .5;
        }
        private void spaceXImage_Pointer_Exited(object sender, RoutedEventArgs e)
        {
            spaceXImage.Opacity = 1.0;
        }

        private async void nasaImage_On_Tapped(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://twitter.com/Nasa"));
        }

        private void nasaImage_Pointer_Entered(object sender, RoutedEventArgs e)
        {
            nasaImage.Opacity = .5;
        }
        private void nasaImage_Pointer_Exited(object sender, RoutedEventArgs e)
        {
            nasaImage.Opacity = 1.0;
        }

        //navigation functionality ----------
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
