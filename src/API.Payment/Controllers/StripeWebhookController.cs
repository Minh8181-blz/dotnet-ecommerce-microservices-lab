using API.Payment.Application.Commands;
using API.Payment.Application.StripeIdempotency;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;
using Stripe.Checkout;
using System.IO;
using System.Threading.Tasks;

namespace API.Payment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeWebhookController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<StripeWebhookController> _logger;
        private readonly IConfiguration _configuration;

        public StripeWebhookController(
            IMediator mediator,
            ILogger<StripeWebhookController> logger,
            IConfiguration configuration)
        {
            _mediator = mediator;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost("checkout-completed")]
        public async Task<ActionResult> CheckoutCompleted()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeWhSecret = _configuration.GetValue<string>("Payment:Stripe:WebhookSecret");
                var stripeSignature = Request.Headers["Stripe-Signature"];
                var stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, stripeWhSecret);

                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;
                    var command = new CompleteStripePaymentCommand(session.Id, session.ClientReferenceId);

                    var identifiedCommand = new StripeIdentifiedCommand<CompleteStripePaymentCommand, bool>(command, stripeEvent.Id, stripeEvent.Type);
                    await _mediator.Send(identifiedCommand);
                }
                else
                {
                    _logger.LogError("StripePayment: Invalid event type: {0}", stripeEvent.Type);
                }
                return Ok();
            }
            catch (StripeException ex)
            {
                _logger.LogTrace(ex, ex.Message);
                return BadRequest();
            }
        }
    }
}
