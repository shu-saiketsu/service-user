using FluentValidation;
using MediatR;
using Saiketsu.Service.User.Application.Interfaces;
using Saiketsu.Service.User.Domain.IntegrationEvents;

namespace Saiketsu.Service.User.Application.Users.Command.DeleteUser;

public sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IAuth0Service _auth0Service;
    private readonly IEventBus _eventBus;
    private readonly IValidator<DeleteUserCommand> _validator;

    public DeleteUserCommandHandler(IAuth0Service auth0Service, IValidator<DeleteUserCommand> validator,
        IEventBus eventBus)
    {
        _auth0Service = auth0Service;
        _validator = validator;
        _eventBus = eventBus;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var deleteSuccess = await _auth0Service.DeleteUserAsync(request.Id);

        if (!deleteSuccess) return false;

        var @event = new UserDeletedIntegrationEvent
        {
            Id = request.Id
        };

        _eventBus.Publish(@event);

        return deleteSuccess;
    }
}