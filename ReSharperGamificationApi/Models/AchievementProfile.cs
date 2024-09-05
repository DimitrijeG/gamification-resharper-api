using AutoMapper;
using ReSharperGamificationApi.DTO;

namespace ReSharperGamificationApi.Models
{
    public class AchievementProfile : Profile
    {
        public AchievementProfile()
        {
            CreateMap<Achievement, AchievementDTO>();
            CreateMap<AchievementDTO, Achievement>();
        }
    }
}
