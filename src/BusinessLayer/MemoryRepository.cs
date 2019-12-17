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
            this.env = env;
        }

        private static readonly Dictionary<string, LocaleViewModel> keyValuePairs = new Dictionary<string, LocaleViewModel>()
        {
            {
                "WA", new LocaleViewModel()
                {
                    Id = "WA",
                    SourceUrl = "https://www.dol.wa.gov/vehicleregistration/parkinguse.html",
                    ContentUrl = "WA.html"
                }
            },
            {
                "Seattle", new LocaleViewModel()
                {
                    Id = "Seattle",
                    SourceUrl = "https://www.seattle.gov/transportation/projects-and-programs/programs/parking-program/disabled-parking",
                    ContentUrl = "Seattle.html"
                }
            }
        };

        private readonly IWebHostEnvironment env;

        public Task<LocaleViewModel> LoadAsync(string id)
        {
            if (keyValuePairs.TryGetValue(id, out LocaleViewModel viewModel))
            {
                viewModel.ContentUrl = Path.Combine(env.WebRootPath, viewModel.ContentUrl);
                return Task.FromResult(viewModel);
            }
            return null;
        }
    }
}
