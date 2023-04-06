using MediatR;

namespace Saiketsu.Service.User.Domain.IntegrationEvents;

public sealed class UserDeletedIntegrationEvent : IRequest
{
    public string Id { get; set; } = null!;
}