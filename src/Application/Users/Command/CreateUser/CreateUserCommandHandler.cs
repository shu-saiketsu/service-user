using FluentValidation;
using MediatR;
using Saiketsu.Service.User.Application.Interfaces;
using Saiketsu.Service.User.Domain.Entities;

namespace Saiketsu.Service.User.Application.Users.Command.CreateUser;

public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserEntity?>
{
    private readonly IAuth0Service _auth0Service;
    private readonly IValidator<CreateUserCommand> _validator;

    public CreateUserCommandHandler(IAuth0Service auth0Service, IValidator<CreateUserCommand> validator)
    {
        _auth0Service = auth0Service;
        _validator = validator;
    }

    public async Task<UserEntity?> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var user = await _auth0Service.CreateUserAsync(request.Email, request.Password);

        return user;
    }
}