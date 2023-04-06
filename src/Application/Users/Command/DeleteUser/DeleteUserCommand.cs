using MediatR;

namespace Saiketsu.Service.User.Application.Users.Command.DeleteUser;

public sealed class DeleteUserCommand : IRequest<bool>
{
    public string Id { get; set; } = null!;
}