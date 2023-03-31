using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Saiketsu.Service.User.Domain.Entities;
using Saiketsu.Service.User.Domain.Options;

namespace Saiketsu.Service.User.Infrastructure.Services;

public sealed class Auth0TokenService
{
    private readonly Auth0ManagementOptions _auth0ManagementOptions;
    private readonly Auth0Options _auth0Options;

    public Auth0TokenService(IOptions<Auth0Options> auth0Options,
        IOptions<Auth0ManagementOptions> auth0ManagementOptions)
    {
        _auth0Options = auth0Options.Value;
        _auth0ManagementOptions = auth0ManagementOptions.Value;
    }

    public async Task<Auth0ResponseToken> GetTokenAsync()
    {
        using var client = new HttpClient
        {
            BaseAddress = new Uri(_auth0Options.Domain)
        };

        var data = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", _auth0ManagementOptions.ClientId },
            { "client_secret", _auth0ManagementOptions.ClientSecret },
            { "audience", _auth0Options.Domain + "/api/v2/" }
        };

        var response = await client.PostAsync("/oauth/token", new FormUrlEncodedContent(data));
        response.EnsureSuccessStatusCode();

        var stringResponse = await response.Content.ReadAsStringAsync();
        var responseToken = JsonConvert.DeserializeObject<Auth0ResponseToken>(stringResponse);

        return responseToken!;
    }
}