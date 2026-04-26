using ContextMcp.Api.Models.Auth;

namespace ContextMcp.Api.Interfaces
{
    public interface IUserStore
    {
        Task<User?> GetUserByApiKeyHashAsync(string apiKeyHash);
    }
}
