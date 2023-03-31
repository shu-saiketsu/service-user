using FluentValidation;
using MediatR;
using Saiketsu.Service.User.Application.Interfaces;
using Saiketsu.Service.User.Domain.Entities;

namespace Saiketsu.Service.User.Application.Users.Query.GetUser;

internal sealed class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserEntity?>
{
    private readonly IAuth0Service _auth0Service;
    private readonly IValidator<GetUserQuery> _validator;

    public GetUserQueryHandler(IAuth0Service auth0Service, IValidator<GetUserQuery> validator)
    {
        _auth0Service = auth0Service;
        _validator = validator;
    }

    public async Task<UserEntity?> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var user = await _auth0Service.GetUserAsync(request);

        return user;
    }
}