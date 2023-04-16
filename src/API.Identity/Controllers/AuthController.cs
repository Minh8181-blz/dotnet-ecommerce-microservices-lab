using API.Identity.Application.Commands;
using API.Identity.Application.Dto;
using API.Identity.Infrastructure;
using API.Identity.ViewModels;
using Application.Base.SeedWork;
using Base.API.Filters.ActionFilters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.CommunicationStandard.Models;
using System;
using System.Threading.Tasks;

namespace API.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly AuthService _authService;

        public AuthController(
            IMediator mediator,
            AuthService authService)
        {
            _mediator = mediator;
            _authService = authService;
        }

        [HttpPost("sign-up-customer")]
        [ValidateRequestId]
        [ValidateModel]
        public async Task<ActionResult> SignUpCustomer([FromBody] SignUpViewModel model, [FromHeader(Name = "x-requestid")] Guid requestId)
        {
            var response = new ActionResultModel();

            var command = new SignUpAsCustomerCommand(model.Email, model.Password);
            var identifiedCommand = new IdentifiedCommand<SignUpAsCustomerCommand, SignUpResultDto>(command, requestId);

            var result = await _mediator.Send(identifiedCommand);

            response.Code = result.Code;
            response.Message = result.Message;

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            response.Data = new SignUpResultViewModel
            {
                User = new UserViewModel
                {
                    Id = result.User.Id,
                    Email = result.User.Email
                }
            };

            return Ok(response);
        }
    }
}
