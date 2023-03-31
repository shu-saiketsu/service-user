using FluentValidation;

namespace Saiketsu.Service.User.Application.Users.Command.CreateUser;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress();

        RuleFor(p => p.Password)
            .NotEmpty()
            .MinimumLength(8)
            .MaximumLength(16)
            .Matches(@"[A-Z]+")
            .Matches(@"[a-z]+")
            .Matches(@"[0-9]+")
            .Matches(@"[][""!@$%^&*(){}:;<>,.?/+_=|'~\\-]");
    }
}