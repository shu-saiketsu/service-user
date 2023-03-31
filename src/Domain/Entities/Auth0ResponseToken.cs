using Newtonsoft.Json;

namespace Saiketsu.Service.User.Domain.Entities;

public sealed class Auth0ResponseToken
{
    [JsonProperty("access_token")] public string AccessToken { get; set; } = null!;
}