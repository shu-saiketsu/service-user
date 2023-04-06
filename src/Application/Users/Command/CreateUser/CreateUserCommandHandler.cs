using FluentValidation;
using MediatR;
using Saiketsu.Service.User.Application.Interfaces;
using Saiketsu.Service.User.Domain.Entities;
using Saiketsu.Service.User.Domain.IntegrationEvents;

namespace Saiketsu.Service.User.Application.Users.Command.CreateUser;

public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserEntity?>
{
    private readonly IAuth0Service _auth0Service;
    private readonly IEventBus _eventBus;
    private readonly IValidator<CreateUserCommand> _validator;

    public CreateUserCommandHandler(IAuth0Service auth0Service, IValidator<CreateUserCommand> validator,
        IEventBus eventBus)
    {
        _auth0Service = auth0Service;
        _validator = validator;
        _eventBus = eventBus;
    }

    public async Task<UserEntity?> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var user = await _auth0Service.CreateUserAsync(request.Email, request.Password);

        if (user == null) return null;

        var @event = new UserCreatedIntegrationEvent
        {
            Id = user.Id
        };

        _eventBus.Publish(@event);

        return user;
    }
}