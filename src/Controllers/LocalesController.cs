using Microsoft.AspNetCore.Mvc;
using ParkingAbilityServer.BusinessLayer;
using ParkingAbilityServer.Models;
using System;
using System.Threading.Tasks;

namespace ParkingAbilityServer.Controllers
{
    [Route("locales")]
    public class LocalesController : Controller
    {
        private readonly IRepository repository;

        public LocalesController(IRepository repository)
        {
            this.repository = repository;
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> LocaleAsync(string id)
        {
            ViewData["Title"] = id;
            LocaleViewModel viewModel = await repository.LoadAsync(id);

            if (DateTime.UtcNow.Ticks % 2 == 0)
            {
                return View("LocalesA", viewModel);
            }
            else
            {
                return View("LocalesB", viewModel);
            }
        }
    }
}
