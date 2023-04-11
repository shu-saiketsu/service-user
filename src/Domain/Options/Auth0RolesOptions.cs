namespace Saiketsu.Service.User.Domain.Options;

public sealed class Auth0RolesOptions
{
    public const string Position = "Auth0:Roles";

    public string Administrator { get; set; } = null!;

    public object? this[string propertyName]
    {
        get
        {
            var type = typeof(Auth0RolesOptions);
            var propInfo = type.GetProperty(propertyName);

            return propInfo?.GetValue(this, null);
        }
    }
}