using Microsoft.AspNetCore.Mvc;
using ParkingAbilityServer.Models;

namespace ParkingAbilityServer.Controllers
{
    [Route("api/locales/{id}")]
    [ApiController]
    public class LocationsApiController : ControllerBase
    {
        [HttpPost]
        [Route("locations")]
        public IActionResult PostAsync([FromRoute] string id, [FromBody] LocationRequest locationRequest)
        {
            return StatusCode(201);
        }
    }
}
