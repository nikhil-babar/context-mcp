using System.Text.Json.Serialization;

namespace ContextMcp.Api.Models.Auth
{
    public class UserApiKey
    {
        public required string Intent { get; set; }
        public required string Key { get; set; }
    }

    public class User
    {
        [JsonPropertyName("user_id")]
        public required string UserId { get; set; }

        [JsonPropertyName("user_name")]
        public required string UserName { get; set; }

        [JsonPropertyName("user_email")]
        public required string UserEmail { get; set; }

        [JsonPropertyName("api_keys")]
        internal List<UserApiKey> ApiKeys { get; set; } = new List<UserApiKey>();
    }
}
