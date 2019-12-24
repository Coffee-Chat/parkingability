using ParkingAbilityServer.Models;
using System.Threading.Tasks;

namespace ParkingAbilityServer.BusinessLayer
{
    public interface ILocationsRepository
    {
        Task CreateAsync(Location location);
    }
}
