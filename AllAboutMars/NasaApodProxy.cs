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
    class NasaApodProxy
    {
        public static async Task<RootObject> GetAPOD(string key)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(String.Format("https://api.nasa.gov/planetary/apod?api_key={0}", key));
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(RootObject));
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(result));
            var data = (RootObject)serializer.ReadObject(memoryStream);
            return data;
        }

        [DataContract]
        public class RootObject
        {
            [DataMember]
            public string copyright { get; set; }

            [DataMember]
            public string date { get; set; }

            [DataMember]
            public string explanation { get; set; }

            [DataMember]
            public string hdurl { get; set; }

            [DataMember]
            public string media_type { get; set; }

            [DataMember]
            public string service_version { get; set; }

            [DataMember]
            public string title { get; set; }

            [DataMember]
            public string url { get; set; }
        }
    }
}
