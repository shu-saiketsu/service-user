using MediatR;
using Saiketsu.Service.User.Domain.Entities;
using Saiketsu.Service.User.Domain.Enums;

namespace Saiketsu.Service.User.Application.Users.Command.CreateUser;

public sealed class CreateUserCommand : IRequest<UserEntity?>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public RoleEnum? Role { get; set; }
}