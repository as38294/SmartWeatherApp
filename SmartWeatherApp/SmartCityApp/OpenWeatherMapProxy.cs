using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.IO;

namespace SmartCityApp
{
    class OpenWeatherMapProxy
    {
        public async static Task<Rootobject> GetWeather(double lat, double lon)
        {
            var http = new HttpClient();
            string url = String.Format("http://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&units=metric&appid=b1b15e88fa797225412429c1c50c122a", lat, lon);
            var response = await http.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            var serializer = new DataContractJsonSerializer(typeof(Rootobject));

            var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));

            var data = (Rootobject)serializer.ReadObject(ms);
            return data;
        }
    }

    [DataContract]
    public class Rootobject
    {
        [DataMember]
        public Coord coord { get; set; }

        [DataMember]
        public Weather[] weather { get; set; }

        [DataMember]
        public string _base { get; set; }

        [DataMember]
        public Main main { get; set; }

        [DataMember]
        public Wind wind { get; set; }

        [DataMember]
        public Clouds clouds { get; set; }

        [DataMember]
        public int dt { get; set; }

        [DataMember]
        public Sys sys { get; set; }

        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string name { get; set; }

        [DataMember]
        public int cod { get; set; }
    }

    [DataContract]
    public class Coord
    {
        [DataMember]
        public float lon { get; set; }

        [DataMember]
        public float lat { get; set; }
    }

    [DataContract]
    public class Main
    {
        [DataMember]
        public float temp { get; set; }

        [DataMember]
        public float pressure { get; set; }

        [DataMember]
        public int humidity { get; set; }

        [DataMember]
        public float temp_min { get; set; }

        [DataMember]
        public float temp_max { get; set; }

        [DataMember]
        public float sea_level { get; set; }

        [DataMember]
        public float grnd_level { get; set; }
    }

    [DataContract]
    public class Wind
    {
        [DataMember]
        public float speed { get; set; }

        [DataMember]
        public float deg { get; set; }
    }

    [DataContract]
    public class Clouds
    {
        [DataMember]
        public int all { get; set; }
    }

    [DataContract]
    public class Sys
    {
        [DataMember]
        public float message { get; set; }

        [DataMember]
        public string country { get; set; }

        [DataMember]
        public int sunrise { get; set; }

        [DataMember]
        public int sunset { get; set; }
    }

    [DataContract]
    public class Weather
    {
        [DataMember]
        public int id { get; set; }

        [DataMember]
        public string main { get; set; }

        [DataMember]
        public string description { get; set; }

        [DataMember]
        public string icon { get; set; }
    }

}
