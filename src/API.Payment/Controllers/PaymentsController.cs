using API.Payment.Application.Commands;
using API.Payment.Application.Dto;
using API.Payment.ViewModels;
using Application.Base.SeedWork;
using Base.API.Filters.ActionFilters;
using IdentityModel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace API.Payment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("pay-order-stripe")]
        [ValidateRequestId]
        [Authorize(Roles = "Customer")]
        public async Task<PayForOrderWithStripeResultDto> PayForOrderByStripe([FromBody] PayForOrderViewModel model, [FromHeader(Name = "x-requestid")] Guid requestId)
        {
            var customerId = int.Parse(User.Claims.First(x => x.Type == JwtClaimTypes.Id).Value);

            var command = new PayForOrderWithStripeCommand(customerId, model.OrderId,
                model.SuccessRedirectUrl, model.CancelRedirectUrl);

            var identifiedCommand = new IdentifiedCommand<PayForOrderWithStripeCommand, PayForOrderWithStripeResultDto>(command, requestId);
            var result = await _mediator.Send(identifiedCommand);

            return result;
        }
    }
}
