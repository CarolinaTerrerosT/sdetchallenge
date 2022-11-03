using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SdetChallengeAutomation.Models
{
    public class WeatherMeasurement
    {
       
        [JsonProperty("station_name")]
        public string station_name { get; set; }
        [JsonProperty("measurement_timestamp")]
        public string measurement_timestamp { get; set; }
        [JsonProperty("air_temperature")]

        public string air_temperature { get; set; }
        [JsonProperty("wet_bulb_temperature")]
        public string wet_bulb_temperature { get; set; }
        [JsonProperty("humidity")]

        public string humidity { get; set; }
        [JsonProperty("rain_intensity")]
        public string rain_intensity { get; set; }
        [JsonProperty("interval_rain")]

        public string interval_rain { get; set; }
        [JsonProperty("total_rain")]

        public string total_rain { get; set; }
        [JsonProperty("precipitation_type")]

        public string precipitation_type { get; set; }
        [JsonProperty("wind_direction")]

        public string wind_direction { get; set; }
        [JsonProperty("wind_speed")]

        public string wind_speed { get; set; }
        [JsonProperty("maximum_wind_speed")]

        public string maximum_wind_speed { get; set; }
        [JsonProperty("barometric_pressure")]

        public string barometric_pressure { get; set; }
        [JsonProperty("solar_radiation")]

        public string solar_radiation { get; set; }
        [JsonProperty("heading")]

        public string heading { get; set; }
        [JsonProperty("battery_life")]
        public string battery_life { get; set; }
        [JsonProperty("measurement_timestamp_label")]
        public string measurement_timestamp_label { get; set; }
        [JsonProperty("measurement_id")]
        public string measurement_id { get; set; }
    }
}
