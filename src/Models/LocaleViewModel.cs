using System.IO;

namespace ParkingAbilityServer.Models
{
    public class LocaleViewModel
    {
        public string Id { get; set; }

        public string SourceUrl { get; set; }

        public string ContentUrl { get; set; }

        public string ReadContent()
        {
            if (string.IsNullOrEmpty(ContentUrl))
            {
                return "Sorry this Id doesn't exist.";
            }

            return File.ReadAllText(Path.GetFullPath(ContentUrl));
        }
    }
}
