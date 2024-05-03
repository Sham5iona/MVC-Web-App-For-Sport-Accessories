using AutoMapper;
using Sport_Accessories.Areas.Identity.Models;
using Sport_Accessories.Models;
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

            CreateMap<User, EditUserViewModel>();

            CreateMap<EditUserViewModel, User>()
                .ForMember(dest => dest.FileName, opt => opt.Ignore())

                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());


            CreateMap<AddUserViewModel, User>()
                //add temporary value to filename until the profile picture attaches
                .ForMember(dest => dest.FileName, opt =>
                opt.MapFrom(src => string.Empty));

            CreateMap<OfferViewModel, Product>()

                //explicitly assign the new price column to null when mapping
                .ForMember(dest => dest.NewPrice, opt =>
                opt.MapFrom(src => (decimal?)null))

                .ForMember(dest => dest.Viewers, opt => opt.MapFrom(src => 0))

                .ForMember(dest => dest.CategoryId, opt => opt.Ignore())

                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            CreateMap<Product, OfferViewModel>()

                .ForMember(dest => dest.CategoryName, opt =>
                opt.MapFrom(src => src.Category.CategoryName))

                .ForMember(dest => dest.FileName, opt =>
                opt.MapFrom(src => src.Photo.FileName));

            CreateMap<Product, Category>();

            CreateMap<Category, CategoryViewModel>();

            CreateMap<CategoryViewModel, Category>();
        }
    }
}
