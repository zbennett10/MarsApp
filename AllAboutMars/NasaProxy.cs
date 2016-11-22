using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace AllAboutMars
{
    class NasaProxy
    {
        //list of possible rovers: curiosity, opportunity, spirit
        public async static Task<RootObject> GetNasaData(string rover, int maxSol) {
            var resources = new Windows.ApplicationModel.Resources.ResourceLoader("Resources");
            var token = resources.GetString("nasaToken");

            HttpClient http = new HttpClient();
            var response = await http.GetAsync(String.Format("https://api.nasa.gov/mars-photos/api/v1/rovers/{0}/photos?sol={1}&api_key={2}",rover, maxSol, token));
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(RootObject));
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (RootObject)serializer.ReadObject(memoryStream);
           

            return data;
        }

        [DataContract]
        public class Camera
        {
            [DataMember]
            public int id { get; set; }

            [DataMember]
            public string name { get; set; }

            [DataMember]
            public int rover_id { get; set; }

            [DataMember]
            public string full_name { get; set; }
        }

        [DataContract]
        public class Camera2
        {
            [DataMember]
            public string name { get; set; }

            [DataMember]
            public string full_name { get; set; }
        }

        [DataContract]
        public class Rover
        {
            [DataMember]
            public int id { get; set; }

            [DataMember]
            public string name { get; set; }

            [DataMember]
            public string landing_date { get; set; }

            [DataMember]
            public string launch_date { get; set; }

            [DataMember]
            public string status { get; set; }

            [DataMember]
            public int max_sol { get; set; }

            [DataMember]
            public string max_date { get; set; }

            [DataMember]
            public int total_photos { get; set; }

            [DataMember]
            public List<Camera2> cameras { get; set; }
        }

        [DataContract]
        public class Photo
        {
            [DataMember]
            public int id { get; set; }

            [DataMember]
            public int sol { get; set; }

            [DataMember]
            public Camera camera { get; set; }

            [DataMember]
            public string img_src { get; set; }

            [DataMember]
            public string earth_date { get; set; }

            [DataMember]
            public Rover rover { get; set; }
        }

        [DataContract]
        public class RootObject
        {
            [DataMember]
            public List<Photo> photos { get; set; }
        }
    }
}
