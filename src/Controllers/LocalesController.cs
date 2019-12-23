using Microsoft.AspNetCore.Mvc;
using ParkingAbilityServer.BusinessLayer;
using ParkingAbilityServer.Models;
using System.Threading.Tasks;

namespace ParkingAbilityServer.Controllers
{
    [Route("locales")]
    public class LocalesController : Controller
    {
        private readonly IRepository repository;
        private readonly IFlightProvider flightProvider;

        public LocalesController(IRepository repository, IFlightProvider flightProvider)
        {
            this.repository = repository;
            this.flightProvider = flightProvider;
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> LocaleAsync(string id)
        {
            ViewData["Title"] = id;
            LocaleViewModel viewModel = await repository.LoadAsync(id);
            ViewData["Flight"] = await flightProvider.CalculateFlightAsync(id);

            return View("Locales", viewModel);
        }
    }
}
