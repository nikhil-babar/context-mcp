using Microsoft.AspNetCore.Authentication;

namespace ContextMcp.Api.Config
{
    public class ApiKeyAuthOptions : AuthenticationSchemeOptions
    {
        public string HeaderName { get; set; } = "x-api-key";
    }
}
