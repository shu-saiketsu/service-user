using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Saiketsu.Service.User.Application.Users.Command.DeleteUser
{
    public sealed class DeleteUserCommand : IRequest<bool>
    {
        public string Id { get; set; } = null!;
    }
}
