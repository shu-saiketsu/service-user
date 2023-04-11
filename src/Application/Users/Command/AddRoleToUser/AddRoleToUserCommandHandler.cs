using FluentValidation;
using MediatR;
using Saiketsu.Service.User.Application.Interfaces;

namespace Saiketsu.Service.User.Application.Users.Command.AddRoleToUser;

public sealed class AddRoleToUserCommandHandler : IRequestHandler<AddRoleToUserCommand, bool>
{
    private readonly IAuth0Service _auth0Service;
    private readonly IValidator<AddRoleToUserCommand> _validator;

    public AddRoleToUserCommandHandler(IAuth0Service auth0Service, IValidator<AddRoleToUserCommand> validator)
    {
        _auth0Service = auth0Service;
        _validator = validator;
    }

    public async Task<bool> Handle(AddRoleToUserCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var success = await _auth0Service.AddRoleToUserAsync(request);

        return success;
    }
}