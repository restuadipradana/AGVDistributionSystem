using System.Threading.Tasks;
using AGVDistributionSystem.DTO;

namespace AGVDistributionSystem._Services.Interfaces
{
    public interface IAuthService
    {
         Task<UserHasLoggedDTO> GetUser(string username, string password);
    }
}