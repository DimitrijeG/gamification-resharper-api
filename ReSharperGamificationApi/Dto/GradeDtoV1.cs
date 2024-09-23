using System.Text.Json.Serialization;

namespace ReSharperGamificationApi.Dto
{
    public class GradeDtoV1
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("points")]
        public double Points { get; set; }
        [JsonPropertyName("group")]
        public GroupDtoV1 Group { get; set; } = null!;
    }
}
