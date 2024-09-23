using System.Text.Json.Serialization;

namespace ReSharperGamificationApi.Dto;

public class GroupDtoV1
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}