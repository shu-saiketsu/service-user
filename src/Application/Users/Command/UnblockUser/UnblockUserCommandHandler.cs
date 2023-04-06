using FluentValidation;
using MediatR;
using Saiketsu.Service.User.Application.Interfaces;

namespace Saiketsu.Service.User.Application.Users.Command.UnblockUser;

public sealed class UnblockUserCommandHandler : IRequestHandler<UnblockUserCommand, bool>
{
    private readonly IAuth0Service _auth0Service;
    private readonly IValidator<UnblockUserCommand> _validator;

    public UnblockUserCommandHandler(IAuth0Service auth0Service, IValidator<UnblockUserCommand> validator)
    {
        _auth0Service = auth0Service;
        _validator = validator;
    }

    public async Task<bool> Handle(UnblockUserCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var unblockedSuccessfully = await _auth0Service.UnblockUserAsync(request.Id);

        return unblockedSuccessfully;
    }
}