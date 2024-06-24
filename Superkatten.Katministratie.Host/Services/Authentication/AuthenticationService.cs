using Microsoft.AspNetCore.Components;
using Superkatten.Katministratie.Contract.Authenticate;
using Superkatten.Katministratie.Host.Entities;
using Superkatten.Katministratie.Host.LocalStorage;
using Superkatten.Katministratie.Host.Services.Http;

namespace Superkatten.Katministratie.Host.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpService _httpService;
        private readonly ILocalStorageService _localStorageService;

        public AuthenticationService(
            IHttpService httpService,
            ILocalStorageService localStorageService
        )
        {
            _httpService = httpService;
            _localStorageService = localStorageService;
        }

        public async Task<User?> AuthenticateUserAsync(string username, string password)
        {
            var uri = $"api/Users/authenticate";
            var authenticateRequest = new AuthenticateRequest
            {
                Username = username,
                Password = password
            };

            try
            {
                var result = await _httpService.Post<AuthenticateResponse>(uri, authenticateRequest);
                return new User
                {
                    Name = result?.Name ?? string.Empty,
                    Email = result?.Email ?? string.Empty,
                    Username = result?.Username ?? string.Empty,
                    Token = result?.Token ?? string.Empty,
                    Id = result?.Id ?? -1
                };
            }
            catch 
            {
                return null;
            }
        }

        public async Task RegisterAsync(string username, string password, string name, string email)
        {
            var uri = $"api/Users/register";
            var registerRequest = new RegisterRequest
            {
                Username = username,
                Password = password,
                Name = name,
                Email = email,
            };

            await _httpService.Post(uri, registerRequest);
        }
    }
}
