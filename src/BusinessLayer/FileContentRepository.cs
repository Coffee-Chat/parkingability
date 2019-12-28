using Microsoft.AspNetCore.Hosting;
using ParkingAbilityServer.Models;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Validation;

namespace ParkingAbilityServer.BusinessLayer
{
    public class FileContentRepository : IContentRepository
    {
        public FileContentRepository(IWebHostEnvironment environment)
        {
            Requires.NotNull(environment, nameof(environment));
            this.environment = environment;
            entityToViewModel = new Dictionary<string, LocaleViewModel>();
        }

        private readonly IWebHostEnvironment environment;
        private readonly Dictionary<string, LocaleViewModel> entityToViewModel;
        private static bool initialized;

        public Task<LocaleViewModel> LoadAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Task.FromResult(default(LocaleViewModel));
            }

            Initialize();

            if (entityToViewModel.TryGetValue(id, out LocaleViewModel viewModel))
            {
                viewModel.ContentUrl = Path.Combine(environment.WebRootPath, $"content/{CultureInfo.CurrentCulture}/{id}.html");
                return Task.FromResult(viewModel);
            }

            return Task.FromResult(default(LocaleViewModel));
        }

        private void Initialize()
        {
            if (initialized)
            {
                return;
            }

            IEnumerable<string> lines = File.ReadLines(Path.Combine(environment.WebRootPath, "content/Manifest.txt"));
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                string entityId = parts[0];
                string url = parts[1];

                var viewModel = new LocaleViewModel()
                {
                    Id = entityId,
                    SourceUrl = url,
                };
                entityToViewModel[viewModel.Id] = viewModel;
            }

            initialized = true;
        }
    }
}
