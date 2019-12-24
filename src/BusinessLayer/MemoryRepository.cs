using Microsoft.AspNetCore.Hosting;
using ParkingAbilityServer.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Validation;

namespace ParkingAbilityServer.BusinessLayer
{
    public class MemoryRepository : IRepository
    {
        public MemoryRepository(IWebHostEnvironment environment)
        {
            Requires.NotNull(environment, nameof(environment));
            EntityToViewModel = new Dictionary<string, LocaleViewModel>()
            {
                {
                    "WA", new LocaleViewModel()
                    {
                        Id = "WA",
                        SourceUrl = "https://www.dol.wa.gov/vehicleregistration/parkinguse.html",
                        ContentUrl =  Path.Combine(environment.WebRootPath, "WA.html")
                    }
                },
                {
                    "Seattle", new LocaleViewModel()
                    {
                        Id = "Seattle",
                        SourceUrl = "https://www.seattle.gov/transportation/projects-and-programs/programs/parking-program/disabled-parking",
                        ContentUrl = Path.Combine(environment.WebRootPath, "Seattle.html")
                    }
                }
            };
        }

        private readonly Dictionary<string, LocaleViewModel> EntityToViewModel;

        public Task<LocaleViewModel> LoadAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Task.FromResult(default(LocaleViewModel));
            }

            if (EntityToViewModel.TryGetValue(id, out LocaleViewModel viewModel))
            {
                return Task.FromResult(viewModel);
            }
            return Task.FromResult(default(LocaleViewModel));
        }
    }
}
