using BOTrasedV3.Models;

namespace BOTrasedV3.Interfaces
{
    public interface IUserDataService
    {
        Task<bool> CreateUserFromId(string userId);
        Task<bool> WriteUser(UserData user);
    }
}
