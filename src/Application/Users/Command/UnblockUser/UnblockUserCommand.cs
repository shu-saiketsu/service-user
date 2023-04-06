using MediatR;

namespace Saiketsu.Service.User.Application.Users.Command.UnblockUser;

public sealed class UnblockUserCommand : IRequest<bool>
{
    public string Id { get; set; } = null!;
}