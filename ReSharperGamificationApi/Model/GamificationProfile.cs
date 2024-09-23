using AutoMapper;
using ReSharperGamificationApi.Dto;

namespace ReSharperGamificationApi.Model;

public class GamificationProfile : Profile
{
    public GamificationProfile()
    {
        CreateMap<User, UserDtoV1>();
        CreateMap<Group, GroupDtoV1>();
        CreateMap<Grade, GradeDtoV1>();
        CreateMap<Achievement, AchievementDtoV1>();
    }
}