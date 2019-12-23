using System.Threading.Tasks;

namespace ParkingAbilityServer.BusinessLayer
{
    public interface IFlightProvider
    {
        Task<string> CalculateFlightAsync(string view);
    }
}