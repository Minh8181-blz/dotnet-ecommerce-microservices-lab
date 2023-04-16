using API.Identity.Application.Dto;
using API.Identity.Domain;
using Application.Base.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Service.CommunicationStandard.Const;
using System.Threading;
using System.Threading.Tasks;

namespace API.Identity.Application.Commands
{
    public class ValidateUserCredentialsCommandHandler : IRequestHandler<ValidateUserCredentialsCommand, UserCredentialsValidationResultDto>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<SignUpAsCustomerCommandHandler> _logger;

        public ValidateUserCredentialsCommandHandler(UserManager<AppUser> userManager, ILogger<SignUpAsCustomerCommandHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<UserCredentialsValidationResultDto> Handle(ValidateUserCredentialsCommand request, CancellationToken cancellationToken)
        {
            var result = new UserCredentialsValidationResultDto();

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                result.Code = ActionCode.BadCommand;
                result.Message = "Invalid email or password";
                return result;
            }

            bool validPassword = await _userManager.CheckPasswordAsync(user, request.Password);

            result.Succeeded = validPassword;

            if (validPassword)
            {
                result.Code = ActionCode.Success;
                result.User = user;
            }
            else
            {
                result.Code = ActionCode.BadCommand;
                result.Message = "Invalid email or password";
            }

            return result;
        }
    }

    public class GetTokenCommandHandlerIdentifiedCommandHandler : IdentifiedCommandHandler<ValidateUserCredentialsCommand, UserCredentialsValidationResultDto>
    {
        public GetTokenCommandHandlerIdentifiedCommandHandler(
            IMediator mediator,
            IRequestManager requestManager)
            : base(mediator, requestManager)
        {
        }

        protected override UserCredentialsValidationResultDto CreateResultForDuplicateRequest()
        {
            var result = new UserCredentialsValidationResultDto
            {
                Succeeded = false,
                Code = ActionCode.DuplicateCommand,
                Message = "Request is already handled"
            };

            return result;
        }
    }
}
