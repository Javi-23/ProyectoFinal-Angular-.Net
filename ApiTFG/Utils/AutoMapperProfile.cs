using ApiTFG.Dtos;
using ApiTFG.Models;
using AutoMapper;

namespace ApiTFG.Utils
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region Post
            CreateMap<Posts, PostDTO>().ReverseMap();
            #endregion Post

            #region UserAndPostDto
            CreateMap<UserAndPostDto, AppUser>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ReverseMap();
            #endregion UserAndPostDto

            #region
            CreateMap<Comment, CommentDTO>().ReverseMap();
            CreateMap<CommentDTO, AppUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ReverseMap();
            #endregion


        }
    }
}
