using MediatR;

namespace Saiketsu.Service.User.Application.Users.Command.BlockUser;

public sealed class BlockUserCommand : IRequest<bool>
{
    public string Id { get; set; } = null!;
}