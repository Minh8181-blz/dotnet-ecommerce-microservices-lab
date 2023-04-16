using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Service.CommunicationStandard.Const;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Utilities.NewtonsoftSerializer;
using Website.MarketingSite.Configurations;
using Website.MarketingSite.Models.Dtos;
using Website.MarketingSite.Models.ViewModels.Auth;

namespace Website.MarketingSite.Services
{
    public class AuthService : HttpServiceBase
    {
        private readonly ILogger<AuthService> _logger;
        private readonly ApiEndpointConfiguration _endpointConfiguration;
        private readonly IdentityClientConfiguration _identityConfiguration;

        public AuthService(
            HttpClient client,
            ApiEndpointConfiguration endpointConfiguration,
            IOptions<IdentityClientConfiguration> identityConfigSection,
            ILogger<AuthService> logger) : base(client)
        {
            _endpointConfiguration = endpointConfiguration;
            _identityConfiguration = identityConfigSection.Value;

            Client.BaseAddress = new Uri(_endpointConfiguration.IdentityOrigin);

            _logger = logger;
        }

        public async Task<SignUpResultDto> SignUp(SignUpViewModel model)
        {
            var result = new SignUpResultDto();

            try
            {
                var response = await PostActionAsync(_endpointConfiguration.SignUp, model);

                if (response == null)
                {
                    result.Message = "An error occured";
                    return result;
                }

                if (response.Code == ActionCode.Created)
                {
                    result.Succeeded = true;

                    var parsedData =  JsonConvert.DeserializeObject<SignUpResponseData>(response.Data.ToString());
                    result.User = parsedData.User;
                }
                else
                {
                    result.Message = response.Message;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result.Message = "An error occured";
            }

            return result;
        }

        public async Task<LoginResultDto> Login(LoginViewModel model)
        {
            var result = new LoginResultDto();

            try
            {
                var request = InitRequest(HttpMethod.Post, _endpointConfiguration.GetIdentityToken);

                var requestBody = new Dictionary<string, string>
                {
                    { "grant_type", "password" },
                    { "scope", string.Format("{0} offline_access", _identityConfiguration.Scope) },
                    { "client_id", _identityConfiguration.ClientId },
                    { "client_secret", _identityConfiguration.ClientSecret },
                    { "username", model.Email },
                    { "password", model.Password }
                };

                request.Content = new FormUrlEncodedContent(requestBody);

                var response = await Client.SendAsync(request);

                if (response == null)
                {
                    result.Message = "An error occured";
                    return result;
                }

                var raw = await response.Content.ReadAsStringAsync();
                

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result.Succeeded = true;

                    var data = JsonConvert.DeserializeObject<LoginResponseData>(raw, NewtonJsonSerializerSettings.SNAKE);
                    result.AccessToken = data.AccessToken;
                    result.RefreshToken = data.RefreshToken;
                    result.TokenExpiresIn = data.ExpiresIn;
                }
                else
                {
                    result.Message = "Invalid email or password";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result.Message = "An error occured";
            }

            return result;
        }

        public async Task<LoginResultDto> GetRefreshToken(string refreshToken)
        {
            var result = new LoginResultDto();

            try
            {
                var request = InitRequest(HttpMethod.Post, _endpointConfiguration.GetIdentityToken);

                var requestBody = new Dictionary<string, string>
                {
                    { "grant_type", "refresh_token" },
                    { "scope", _identityConfiguration.Scope },
                    { "client_id", _identityConfiguration.ClientId },
                    { "client_secret", _identityConfiguration.ClientSecret },
                    { "refresh_token", refreshToken },
                };

                request.Content = new FormUrlEncodedContent(requestBody);

                var response = await Client.SendAsync(request);

                if (response == null)
                {
                    result.Message = "An error occured";
                    return result;
                }

                var raw = await response.Content.ReadAsStringAsync();


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    result.Succeeded = true;

                    var data = JsonConvert.DeserializeObject<LoginResponseData>(raw, NewtonJsonSerializerSettings.SNAKE);
                    result.AccessToken = data.AccessToken;
                    result.RefreshToken = data.RefreshToken;
                    result.TokenExpiresIn = data.ExpiresIn;
                }
                else
                {
                    result.Message = "Invalid email or password";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                result.Message = "An error occured";
            }

            return result;
        }
    }
}
