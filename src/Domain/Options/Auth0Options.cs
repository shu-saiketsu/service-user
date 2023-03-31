namespace Saiketsu.Service.User.Domain.Options;

public sealed class Auth0Options
{
    public const string Position = "Auth0";

    public string Domain { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string DatabaseConnection { get; set; } = null!;
}