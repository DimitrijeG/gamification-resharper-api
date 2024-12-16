using AutoMapper;
using ReSharperGamificationApi.Dtos;

namespace ReSharperGamificationApi.Models.Context;

public class GamificationProfile : Profile
{
    public GamificationProfile()
    {
        CreateMap<User, UserDtoV1>();
        CreateMap<User, LeaderboardEntry>()
            .ForMember(dest => dest.Position, opt => opt.Ignore());

        CreateMap<Group, GroupDtoV1>();
        CreateMap<Goal, GoalDtoV1>();
        CreateMap<Achievement, AchievementResponseDtoV1>();
    }
}