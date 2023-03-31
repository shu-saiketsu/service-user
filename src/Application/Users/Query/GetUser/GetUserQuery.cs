using MediatR;
using Saiketsu.Service.User.Domain.Entities;

namespace Saiketsu.Service.User.Application.Users.Query.GetUser;

public sealed class GetUserQuery : IRequest<UserEntity?>
{
    public string Id { get; set; } = null!;
}