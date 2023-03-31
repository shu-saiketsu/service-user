using MediatR;
using Saiketsu.Service.User.Domain.Entities;

namespace Saiketsu.Service.User.Application.Users.Command.CreateUser;

public sealed class CreateUserCommand : IRequest<UserEntity?>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}