namespace Saiketsu.Service.User.Domain.Options;

public sealed class Auth0ManagementOptions
{
    public const string Position = "Auth0:Management";

    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
}