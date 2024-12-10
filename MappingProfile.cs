using AutoMapper;
using real_estate_api.DTOs;
using real_estate_api.Models;
namespace real_estate_api
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterRequestDTO, User>();
            CreateMap<User, UserDTO>();
            CreateMap<UpdateUserRqDTO, User>();
            CreateMap<PostCreateDTO, Post>();
            CreateMap<Post, PostResponseDTO>().ForMember(dest => dest.User, opt => opt.MapFrom(src => new UserDTOForPost
            {
                Username = src.User.Username,
                Avatar = src.User.Avatar
            }));
        }
    }
}
