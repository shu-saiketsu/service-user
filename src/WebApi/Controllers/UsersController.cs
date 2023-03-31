using MediatR;
using Microsoft.AspNetCore.Mvc;
using Saiketsu.Service.User.Application.Users.Command.CreateUser;
using Saiketsu.Service.User.Application.Users.Command.DeleteUser;
using Saiketsu.Service.User.Application.Users.Query.GetUser;
using Saiketsu.Service.User.Application.Users.Query.GetUsers;

namespace Saiketsu.Service.User.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var query = new GetUserQuery { Id = id };

        var user = await _mediator.Send(query);

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetUsersQuery();

        var users = await _mediator.Send(query);

        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
    {
        var user = await _mediator.Send(command);

        if (user == null)
            return BadRequest();

        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var request = new DeleteUserCommand { Id = id };

        var response = await _mediator.Send(request);

        if (response) 
            return Ok();

        return BadRequest();
    }
}