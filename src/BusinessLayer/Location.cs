using Newtonsoft.Json;
using System;

namespace ParkingAbilityServer.BusinessLayer
{
    [JsonObject]
    public class Location
    {
        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; set; }

        [JsonProperty(PropertyName = "clientTimestamp")]
        public long ClientTimestamp { get; set; }

        [JsonProperty(PropertyName = "parentEntity")]
        public string ParentEntity { get; set; }

        [JsonProperty(PropertyName = "hourlyTimestampUtc")]
        public DateTime HourlyTimestampUtc { get; set; }

        [JsonProperty(PropertyName = "userAgent")]
        public string UserAgent { get; set; }

        [JsonProperty(PropertyName = "session")]
        public string Session { get;  set; }
    }
}
