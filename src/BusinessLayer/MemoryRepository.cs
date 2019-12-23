using Microsoft.AspNetCore.Hosting;
using ParkingAbilityServer.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ParkingAbilityServer.BusinessLayer
{
    public class MemoryRepository : IRepository
    {
        public MemoryRepository(IWebHostEnvironment env)
        {
            EntityToViewModel = new Dictionary<string, LocaleViewModel>()
            {
                {
                    "WA", new LocaleViewModel()
                    {
                        Id = "WA",
                        SourceUrl = "https://www.dol.wa.gov/vehicleregistration/parkinguse.html",
                        ContentUrl =  Path.Combine(env.WebRootPath, "WA.html")
                    }
                },
                {
                    "Seattle", new LocaleViewModel()
                    {
                        Id = "Seattle",
                        SourceUrl = "https://www.seattle.gov/transportation/projects-and-programs/programs/parking-program/disabled-parking",
                        ContentUrl = Path.Combine(env.WebRootPath, "Seattle.html")
                    }
                }
            };
        }

        private readonly Dictionary<string, LocaleViewModel> EntityToViewModel;

        public Task<LocaleViewModel> LoadAsync(string id)
        {
            if (EntityToViewModel.TryGetValue(id, out LocaleViewModel viewModel))
            {
                return Task.FromResult(viewModel);
            }
            return null;
        }
    }
}
