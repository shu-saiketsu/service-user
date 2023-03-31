using FluentValidation;
using MediatR;
using Saiketsu.Service.User.Application.Interfaces;
using Saiketsu.Service.User.Domain.Entities;

namespace Saiketsu.Service.User.Application.Users.Query.GetUsers;

public sealed class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserEntity>>
{
    private readonly IAuth0Service _auth0Service;
    private readonly IValidator<GetUsersQuery> _validator;

    public GetUsersQueryHandler(IAuth0Service auth0Service, IValidator<GetUsersQuery> validator)
    {
        _auth0Service = auth0Service;
        _validator = validator;
    }

    public async Task<List<UserEntity>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var users = await _auth0Service.GetUsersAsync();

        return users;
    }
}