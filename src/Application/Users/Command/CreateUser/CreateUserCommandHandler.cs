using FluentValidation;
using MediatR;
using Saiketsu.Service.User.Application.Interfaces;
using Saiketsu.Service.User.Application.Users.Command.AddRoleToUser;
using Saiketsu.Service.User.Domain.Entities;
using Saiketsu.Service.User.Domain.Enums;
using Saiketsu.Service.User.Domain.IntegrationEvents;

namespace Saiketsu.Service.User.Application.Users.Command.CreateUser;

public sealed class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserEntity?>
{
    private readonly IAuth0Service _auth0Service;
    private readonly IEventBus _eventBus;
    private readonly IMediator _mediator;
    private readonly IValidator<CreateUserCommand> _validator;

    public CreateUserCommandHandler(IAuth0Service auth0Service, IValidator<CreateUserCommand> validator,
        IEventBus eventBus, IMediator mediator)
    {
        _auth0Service = auth0Service;
        _validator = validator;
        _eventBus = eventBus;
        _mediator = mediator;
    }

    public async Task<UserEntity?> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);

        var user = await _auth0Service.CreateUserAsync(request);

        if (user == null) return null;

        if (request.Role != null)
            await _mediator.Send(new AddRoleToUserCommand { UserId = user.Id, Role = (RoleEnum)request.Role },
                cancellationToken);

        var @event = new UserCreatedIntegrationEvent
        {
            Id = user.Id
        };

        _eventBus.Publish(@event);

        return user;
    }
}