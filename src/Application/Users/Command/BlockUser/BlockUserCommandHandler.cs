using FluentValidation;
using MediatR;
using Saiketsu.Service.User.Application.Interfaces;

namespace Saiketsu.Service.User.Application.Users.Command.BlockUser;

public sealed class BlockUserCommandHandler : IRequestHandler<BlockUserCommand, bool>
{
    private readonly IAuth0Service _auth0Service;
    private readonly IValidator<BlockUserCommand> _validator;

    public BlockUserCommandHandler(IAuth0Service auth0Service, IValidator<BlockUserCommand> validator)
    {
        _auth0Service = auth0Service;
        _validator = validator;
    }

    public async Task<bool> Handle(BlockUserCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var blockedSuccessfully = await _auth0Service.BlockUserAsync(request.Id);

        return blockedSuccessfully;
    }
}