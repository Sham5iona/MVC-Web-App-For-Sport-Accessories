using AutoMapper;
using Sport_Accessories.Areas.Identity.Models;
using Sport_Accessories.Services;
using Sport_Accessories.ViewModels;

namespace Sport_Accessories.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<RegisterViewModel, User>()
                .ForMember(dest => dest.FileName, opt =>
                opt.MapFrom(src => "defaultImage.jpg"))
                .ForMember(dest => dest.LastModified_20118018, opt =>
                opt.MapFrom(src => DateTime.UtcNow));

            CreateMap<TwoFactorAuthenticationViewModel, User>();

            CreateMap<UpdateProfilePictureViewModel, User>()
                .ForMember(dest => dest.UserName, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore());

            //ignore the Username when changing the profile picture only
            CreateMap<User, UpdateProfilePictureViewModel>()
                .ForMember(dest => dest.Username, opt => opt.Ignore());

        }
    }
}
