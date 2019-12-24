using System.Threading.Tasks;

namespace ParkingAbilityServer.BusinessLayer
{
    public class MemoryLocationsRepository : ILocationsRepository
    {
        public Task CreateAsync(Location location)
        {
            return Task.CompletedTask;
        }
    }
}
