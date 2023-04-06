using Saiketsu.Service.User.Application.Users.Query.GetUser;
using Saiketsu.Service.User.Domain.Entities;

namespace Saiketsu.Service.User.Application.Interfaces;

public interface IAuth0Service
{
    Task<UserEntity?> GetUserAsync(GetUserQuery query);
    Task<List<UserEntity>> GetUsersAsync();
    Task<UserEntity?> CreateUserAsync(string email, string password);
    Task<bool> DeleteUserAsync(string id);
    Task<bool> BlockUserAsync(string id);
    Task<bool> UnblockUserAsync(string id);
}