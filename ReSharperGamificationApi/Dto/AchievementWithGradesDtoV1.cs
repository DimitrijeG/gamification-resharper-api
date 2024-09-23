using System.Text.Json.Serialization;

namespace ReSharperGamificationApi.Dto;

[method: JsonConstructor]
public class AchievementWithGradesDtoV1(string group, ICollection<string> grades)
{
    [JsonPropertyName("group")]
    public string Group { get; } = group;
    [JsonPropertyName("grades")]
    public ICollection<string> Grades { get; } = grades;
}