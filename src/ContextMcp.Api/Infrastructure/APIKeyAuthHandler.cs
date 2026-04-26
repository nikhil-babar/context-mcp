using ContextMcp.Api.Config;
using ContextMcp.Api.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;

namespace ContextMcp.Api.Infrastructure
{
    public class APIKeyAuthHandler : AuthenticationHandler<ApiKeyAuthOptions>
    {
        private readonly IUserStore _userStore;
        public APIKeyAuthHandler(
            IOptionsMonitor<ApiKeyAuthOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            IUserStore store)
            : base(options, logger, encoder)
        {
            _userStore = store;
        }
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(Options.HeaderName, out var rawKey)
                || StringValues.IsNullOrEmpty(rawKey))
                      return AuthenticateResult.NoResult();

            var keyHash = ComputeHash(rawKey!);

            var user = await _userStore.GetUserByApiKeyHashAsync(keyHash);

            if (user is null)
            {
                // Constant-time delay — prevents timing attacks that reveal
                // whether a key exists vs is revoked
                await Task.Delay(Random.Shared.Next(5, 15));
                return AuthenticateResult.Fail("Invalid API key");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.UserEmail),
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }

        private static string ComputeHash(string raw)
            => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(raw)))
                      .ToLowerInvariant();
    }
}
