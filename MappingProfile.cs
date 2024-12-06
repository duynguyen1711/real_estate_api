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
            CreateMap<UpdateUserDTO,User >()
            .ForMember(dest => dest.Password, opt => opt.Ignore());



        }
    }
}
