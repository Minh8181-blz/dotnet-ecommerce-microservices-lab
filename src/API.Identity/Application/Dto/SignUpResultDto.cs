using API.Identity.Domain;
using Application.Base.SeedWork;

namespace API.Identity.Application.Dto
{
    public class SignUpResultDto : CommandResultModel
    {
        public AppUser User { get; set; }
    }
}
