namespace ReSharperGamificationApi.DTO
{
    public class AchievementWithGradesDTO
    {
        public string Group { get; set; } = string.Empty;
        public ICollection<string> Grades { get; set; } = [];
    }
}
