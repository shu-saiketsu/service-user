using MediatR;
using Saiketsu.Service.User.Domain.Enums;

namespace Saiketsu.Service.User.Application.Users.Command.AddRoleToUser;

public sealed class AddRoleToUserCommand : IRequest<bool>
{
    public string UserId { get; set; } = null!;
    public RoleEnum Role { get; set; }
}