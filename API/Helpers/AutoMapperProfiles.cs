using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<RegisterDto, AppUser>();
        CreateMap<AppUser, UserDto>();
        CreateMap<Group, GroupDto>();
        CreateMap<AppUser, MemberDto>();
        CreateMap<Poll, PollDto>();
        CreateMap<UserGroup, MemberDto>()
            .ForMember(x => x.Id, y => y.MapFrom(z => z.User.Id))
            .ForMember(x => x.KnownAs, y => y.MapFrom(z => z.User.KnownAs));
        CreateMap<GroupCreateDto, Group>();
        CreateMap<PollCreateDto, Poll>();
        CreateMap<Poll, PollDto>();
    }
}
