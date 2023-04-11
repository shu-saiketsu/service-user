namespace Saiketsu.Service.User.Domain.Entities;

public sealed class UserEntity
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool EmailVerified { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CreatedAt { get; set; }
}