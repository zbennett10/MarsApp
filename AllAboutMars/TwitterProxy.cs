using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using LinqToTwitter;

namespace AllAboutMars
{
    class TwitterProxy
    {
        //private static SingleUserAuthorizer authorizer = new SingleUserAuthorizer {
        //    CredentialStore = new SingleUserInMemoryCredentialStore
        //    {
        //        ConsumerKey = "eS5AVLBVb2w0gwuiG53wat08s",
        //        ConsumerSecret = "asdkfjmI3UO4xqwKZmWp91RlclieElXZONCUzZDvxZ6DtASNgFOK7Qgo",
        //        AccessToken = "710985700125048832-RkVkwXqKeWSBKFQQ5bxwTEFeceBAACt",
        //        AccessTokenSecret = "QXbSK9YPbg5dL3Vd52jOw8rlNLq0ScwJ3prTzvIIcRJJ4"
        //    }


        //};

        
        //public static List<Status> list;
        //public static void GetMostRecent10HomeTimeLine()
        //{
        //    var twitterContext = new TwitterContext(authorizer);
            

        //    var tweets = from tweet in twitterContext.Status
        //                 where tweet.Type == StatusType.Home &&
        //                 tweet.Count == 5
        //                 select tweet;

        //    list = tweets.ToList();
        //}


        //public static List<Status> SearchTwitter(string searchTerm)
        //{
        //    var twitterContext2 = new TwitterContext(authorizer);

        //    var srch =
        //       Enumerable.SingleOrDefault((from search in twitterContext2.Search
        //                                   where search.Type == SearchType.Search &&
        //                                   search.Query == searchTerm &&
        //                                   search.Count == 5
        //                                   select search));
        //    if (srch != null && srch.Statuses.Count > 0)
        //    {
        //        return srch.Statuses.ToList();
        //    }

        //    return new List<Status>();
        //}

        
        

        //private async Task<RootObject> GetTwitterData()
        //{
        //    HttpClient http = new HttpClient();
        //    var response = await http.GetAsync();
        //    var result = await response.Content.ReadAsStringAsync();
        //    var serializer = new DataContractJsonSerializer(typeof(RootObject));



        //}


    }
}
