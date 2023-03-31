using MediatR;
using Saiketsu.Service.User.Domain.Entities;

namespace Saiketsu.Service.User.Application.Users.Query.GetUsers;

public sealed class GetUsersQuery : IRequest<List<UserEntity>>
{
}