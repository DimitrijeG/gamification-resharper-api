using AutoMapper;
using ReSharperGamificationApi.Dtos;
using ReSharperGamificationApi.Models.Achievements;

namespace ReSharperGamificationApi.Models;

public class GamificationProfile : Profile
{
    public GamificationProfile()
    {
        CreateMap<User, UserDtoV1>();
        CreateMap<User, LeaderboardEntryDtoV1>();

        CreateMap<Group, GroupDtoV1>();
        CreateMap<Grade, GradeDtoV1>();
        CreateMap<Achievement, AchievementDtoV1>();
    }
}