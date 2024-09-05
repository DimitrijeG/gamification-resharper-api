namespace ReSharperGamificationApi.DTO
{
    public class AchievementDTO
    {
        public long Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Group { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
    }
}
