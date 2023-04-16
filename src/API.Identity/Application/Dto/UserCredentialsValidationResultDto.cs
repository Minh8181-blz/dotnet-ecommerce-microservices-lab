using API.Identity.Domain;
using Application.Base.SeedWork;

namespace API.Identity.Application.Dto
{
    public class UserCredentialsValidationResultDto : CommandResultModel
    {
        public AppUser User { get; set; }
    }
}
