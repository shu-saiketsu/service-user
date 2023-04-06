using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Microsoft.Extensions.Options;
using Saiketsu.Service.User.Application.Interfaces;
using Saiketsu.Service.User.Application.Users.Query.GetUser;
using Saiketsu.Service.User.Domain.Entities;
using Saiketsu.Service.User.Domain.Options;

namespace Saiketsu.Service.User.Infrastructure.Services;

public sealed class Auth0Service : IAuth0Service
{
    private readonly Auth0Options _auth0Options;
    private readonly Auth0TokenService _tokenService;

    public Auth0Service(IOptions<Auth0Options> auth0Options, Auth0TokenService tokenService)
    {
        _auth0Options = auth0Options.Value;
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
                Email = user.Email,
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
                Email = x.Email,
                UpdatedAt = x.UpdatedAt,
                CreatedAt = x.CreatedAt
            }).ToList();
        }
        catch (Exception)
        {
            return new List<UserEntity>();
        }
    }

    public async Task<UserEntity?> CreateUserAsync(string email, string password)
    {
        try
        {
            var client = await GetClientAsync();

            var request = new UserCreateRequest
            {
                Connection = _auth0Options.DatabaseConnection,
                EmailVerified = true,
                VerifyEmail = false,

                Email = email,
                Password = password
            };

            var user = await client.Users.CreateAsync(request);

            return new UserEntity
            {
                Id = user.UserId,
                Email = user.Email
            };
        }
        catch (Exception)
        {
            return null;
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