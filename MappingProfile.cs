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
            CreateMap<PostDetailCreateDTO, PostDetail>();
            CreateMap<PostUpdateDTO, Post>();
            CreateMap<PostDetailUpdateDTO, PostDetail>();
            CreateMap<Post, PostResponseDTO>();
            // Ánh xạ từ User sang UserDTOForPost
            CreateMap<User, UserDTOForPost>();
                
            // Ánh xạ từ PostDetail sang PostDetailCreateDTO
            CreateMap<PostDetail, PostDetailCreateDTO>();
               

        }
    }
}
