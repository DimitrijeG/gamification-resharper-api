using System.Text.Json.Serialization;

namespace ReSharperGamificationApi.Dto
{
    public class UserDtoV1
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = string.Empty;
        [JsonPropertyName("lastName")]
        public string LastName { get; set; } = string.Empty;
    }
}
