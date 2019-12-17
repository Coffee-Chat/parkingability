using ParkingAbilityServer.Models;
using System.Threading.Tasks;

namespace ParkingAbilityServer.BusinessLayer
{
    public interface IRepository
    {
        Task<LocaleViewModel> LoadAsync(string id);
    }
}