using System;
using System.ComponentModel.DataAnnotations;

namespace ParkingAbilityServer.Models
{
    public class LocationRequest
    {
        [Display(Name = "latitude")]
        [Range(-90.0, 90.0, ErrorMessage = "latitude range is between -90 and 90.")]
        public float Latitude { get; set; }

        [Display(Name = "longitude")]
        [Range(-180.0, 180.0, ErrorMessage = "longitude range is between -180 and 180.")]
        public float Longitude { get; set; }

        [Display(Name = "timestamp")]
        [Required]
        [Range(0, 9999999999999, ErrorMessage = "timestamp should be a positive interger.")]
        public long Timestamp { get; set; }
    }
}
