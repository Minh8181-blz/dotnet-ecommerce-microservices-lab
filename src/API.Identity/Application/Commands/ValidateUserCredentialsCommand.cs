using API.Identity.Application.Dto;
using MediatR;

namespace API.Identity.Application.Commands
{
    public class ValidateUserCredentialsCommand : IRequest<UserCredentialsValidationResultDto>
    {
        public ValidateUserCredentialsCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Email { get; }
        public string Password { get; }
    }
}
