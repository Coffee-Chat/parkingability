using ParkingAbilityServer.Models;
using System.Threading.Tasks;

namespace ParkingAbilityServer.BusinessLayer
{
    public interface IContentRepository
    {
        Task<LocaleViewModel> LoadAsync(string id);
    }
}