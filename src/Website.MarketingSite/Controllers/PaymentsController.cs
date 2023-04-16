using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.MarketingSite.Controllers.Bases;
using Website.MarketingSite.Models.ViewModels.Payment;
using Website.MarketingSite.Services;

namespace Website.MarketingSite.Controllers
{
    [Route("[controller]")]
    [Authorize(Roles = "Customer")]
    public class PaymentsController : AppControllerBase
    {
        private readonly PaymentService _paymentService;

        public PaymentsController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("stripe/pay/{orderId}")]
        public async Task<PayForOrderStripeResultViewModel> PayWithStripeAsync(int orderId)
        {
            var jwt = GetAuthorizationJwt();
            var paymentInstruction = await _paymentService.PayOrderWithStripeAsync(orderId, jwt);

            return paymentInstruction;
        }

        [HttpGet("stripe/order-payment-completed")]
        public IActionResult PaymentCompleted()
        {
            return View();
        }

        [HttpGet("stripe/order-payment-canceled")]
        public IActionResult PaymentCanceled()
        {
            return View();
        }
    }
}
