using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Microsoft.Extensions.Options;
using Saiketsu.Service.User.Application.Interfaces;
using Saiketsu.Service.User.Application.Users.Command.AddRoleToUser;
using Saiketsu.Service.User.Application.Users.Command.CreateUser;
using Saiketsu.Service.User.Application.Users.Query.GetUser;
using Saiketsu.Service.User.Domain.Entities;
using Saiketsu.Service.User.Domain.Options;

namespace Saiketsu.Service.User.Infrastructure.Services;

public sealed class Auth0Service : IAuth0Service
{
    private readonly Auth0Options _auth0Options;
    private readonly Auth0RolesOptions _auth0ORolesOptions;
    private readonly Auth0TokenService _tokenService;

    public Auth0Service(IOptions<Auth0Options> auth0Options, IOptions<Auth0RolesOptions> auth0ORolesOptions,
        Auth0TokenService tokenService)
    {
        _auth0Options = auth0Options.Value;
        _auth0ORolesOptions = auth0ORolesOptions.Value;
        _tokenService = tokenService;
    }

    public async Task<UserEntity?> GetUserAsync(GetUserQuery query)
    {
        try
        {
            var client = await GetClientAsync();
            var user = await client.Users.GetAsync(query.Id);

            return new UserEntity
            {
                Id = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                EmailVerified = user.EmailVerified ?? false,
                UpdatedAt = user.UpdatedAt,
                CreatedAt = user.CreatedAt
            };
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<List<UserEntity>> GetUsersAsync()
    {
        try
        {
            var client = await GetClientAsync();

            var request = new GetUsersRequest
            {
                Connection = _auth0Options.DatabaseConnection,
                SearchEngine = "v3"
            };

            var users = await client.Users.GetAllAsync(request);

            if (users == null)
                return new List<UserEntity>();

            return users.Select(x => new UserEntity
            {
                Id = x.UserId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                EmailVerified = x.EmailVerified ?? false,
                UpdatedAt = x.UpdatedAt,
                CreatedAt = x.CreatedAt
            }).ToList();
        }
        catch (Exception)
        {
            return new List<UserEntity>();
        }
    }

    public async Task<UserEntity?> CreateUserAsync(CreateUserCommand command)
    {
        try
        {
            var client = await GetClientAsync();

            var request = new UserCreateRequest
            {
                Connection = _auth0Options.DatabaseConnection,
                EmailVerified = false,
                VerifyEmail = false,

                Email = command.Email,
                Password = command.Password,
                FirstName = command.FirstName,
                LastName = command.LastName
            };

            var user = await client.Users.CreateAsync(request);

            return new UserEntity
            {
                Id = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                EmailVerified = user.EmailVerified ?? false,
                UpdatedAt = user.UpdatedAt,
                CreatedAt = user.CreatedAt
            };
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<bool> AddRoleToUserAsync(AddRoleToUserCommand command)
    {
        try
        {
            var client = await GetClientAsync();

            var roleId = _auth0ORolesOptions[command.Role.ToString()];

            var request = new AssignRolesRequest
            {
                Roles = new[] { roleId?.ToString() }
            };

            await client.Users.AssignRolesAsync(command.UserId, request);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteUserAsync(string id)
    {
        try
        {
            var client = await GetClientAsync();

            await client.Users.DeleteAsync(id);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> BlockUserAsync(string id)
    {
        try
        {
            var client = await GetClientAsync();

            var blockRequest = new UserUpdateRequest
            {
                Blocked = true
            };

            await client.Users.UpdateAsync(id, blockRequest);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> UnblockUserAsync(string id)
    {
        try
        {
            var client = await GetClientAsync();

            var blockRequest = new UserUpdateRequest
            {
                Blocked = false
            };

            await client.Users.UpdateAsync(id, blockRequest);

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private async Task<ManagementApiClient> GetClientAsync()
    {
        var tokenData = await _tokenService.GetTokenAsync();
        var path = _auth0Options.Domain + "/api/v2";

        return new ManagementApiClient(tokenData.AccessToken, new Uri(path));
    }
}