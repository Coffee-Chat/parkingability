using Microsoft.AspNetCore.Mvc;
using ParkingAbilityServer.BusinessLayer;
using ParkingAbilityServer.Models;
using System.Threading.Tasks;
using Validation;

namespace ParkingAbilityServer.Controllers
{
    [Route("api/locales/{id}")]
    [ApiController]
    public class LocationsApiController : ControllerBase
    {
        private readonly ILocationsRepository repository;

        public LocationsApiController(ILocationsRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        [Route("locations")]
        public async Task<IActionResult> PostAsync([FromRoute] string id, 
            [FromBody] LocationRequest locationRequest, 
            [FromHeader] MetadataRequest metadataRequest)
        {
            Requires.NotNull(locationRequest, nameof(locationRequest));
            Requires.NotNull(metadataRequest, nameof(metadataRequest));
            Location location = new Location()
            {
                ClientTimestamp = locationRequest.Timestamp,
                Latitude = locationRequest.Latitude,
                Longitude = locationRequest.Longitude,
                ParentEntity = id,
                UserAgent = metadataRequest.UserAgent,
                Session = metadataRequest.Session
            };
            await repository.CreateAsync(location);

            return StatusCode(201);
        }
    }
}
