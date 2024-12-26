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

            CreateMap<SavedPostDTO, SavedPost>();
            CreateMap<Chat, ChatDTOResponse>()
           .ForMember(dest => dest.UserIDs, opt => opt.MapFrom(src => src.ChatUsers.Select(cu => cu.UserId).ToList()))  // ánh xạ từ ChatUser để lấy danh sách userId
           .ForMember(dest => dest.SeenBy, opt => opt.MapFrom(src => src.SeenByUsers.Select(sb => sb.UserId).ToList())) // ánh xạ từ ChatSeenBy để lấy danh sách người đã xem
           .ForMember(dest => dest.LastMessage, opt => opt.MapFrom(src => src.LastMessage))
           .ForMember(dest => dest.Messages, opt => opt.MapFrom(src => src.Messages)); // ánh xạ danh sách messages

            CreateMap<MessageRequestDTO, Message>();
            CreateMap<Message, MessageResponeDTO>();

        }
    }
}
