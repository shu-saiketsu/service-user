using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Saiketsu.Service.User.Application.Interfaces;
using Saiketsu.Service.User.Application.Users.Command.CreateUser;

namespace Saiketsu.Service.User.Application.Users.Command.DeleteUser
{
    public sealed class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IAuth0Service _auth0Service;
        private readonly IValidator<DeleteUserCommand> _validator;

        public DeleteUserCommandHandler(IAuth0Service auth0Service, IValidator<DeleteUserCommand> validator)
        {
            _auth0Service = auth0Service;
            _validator = validator;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var user = await _auth0Service.DeleteUserAsync(request.Id);

            return user;
        }
    }
}
