using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Sport_Accessories.Areas.Identity.Models;
using Sport_Accessories.Models;
using Sport_Accessories.Services;
using Sport_Accessories.ViewModels;
using System.Security.Claims;

namespace Sport_Accessories.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<RegisterViewModel, User>()

                .ForMember(dest => dest.FileName, opt =>
                opt.MapFrom(src => "defaultImage.jpg"))

                .ForMember(dest => dest.LastModified_20118018, opt => opt
                .MapFrom(src => DateTime.Now));

            CreateMap<User, EditUserViewModel>();

            CreateMap<EditUserViewModel, User>()
                .ForMember(dest => dest.FileName, opt => opt.Ignore())

                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())

                .ForMember(dest => dest.LastModified_20118018, opt => opt
                .MapFrom(src => DateTime.Now));


            CreateMap<AddUserViewModel, User>()
                //add temporary value to filename until the profile picture attaches
                .ForMember(dest => dest.FileName, opt =>
                opt.MapFrom(src => string.Empty))

                .ForMember(dest => dest.LastModified_20118018, opt => opt
                .MapFrom(src => DateTime.Now));

            CreateMap<OfferViewModel, Product>()

                //explicitly assign the new price column to null when mapping
                .ForMember(dest => dest.NewPrice, opt =>
                opt.MapFrom(src => (decimal?)null))

                .ForMember(dest => dest.Viewers, opt => opt.MapFrom(src => 0))

                .ForMember(dest => dest.CategoryId, opt => opt.Ignore())

                .ForMember(dest => dest.UserId, opt => opt.Ignore())

                .ForMember(dest => dest.LastModified_20118018, opt => opt
                .MapFrom(src => DateTime.Now));

            CreateMap<Product, OfferViewModel>()

                .ForMember(dest => dest.CategoryName, opt =>
                opt.MapFrom(src => src.Category.CategoryName))

                .ForMember(dest => dest.FileName, opt =>
                opt.MapFrom(src => src.Photo.FileName));

            CreateMap<Product, Category>()
                .ForMember(dest => dest.LastModified_20118018, opt => opt
                .MapFrom(src => DateTime.Now));

            CreateMap<Category, CategoryViewModel>();

            CreateMap<CategoryViewModel, Category>()
                .ForMember(dest => dest.LastModified_20118018, opt => opt
                .MapFrom(src => DateTime.Now));

            CreateMap<User, Bag>()
                .ForMember(dest => dest.UserId, opt =>
                opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.LastModified_20118018, opt => opt
                .MapFrom(src => DateTime.Now));

            CreateMap<Product, BagProduct>()
                .ForMember(dest => dest.LastModified_20118018, opt => opt
                .MapFrom(src => DateTime.Now));

            CreateMap<Bag, BagProduct>()
                .ForMember(dest => dest.LastModified_20118018, opt => opt
                .MapFrom(src => DateTime.Now));

            CreateMap<AddAdminViewModel, User>()
                .ForMember(dest => dest.LastModified_20118018, opt => opt
                .MapFrom(src => DateTime.Now))

                .ForMember(dest => dest.EmailConfirmed, opt =>
                opt.MapFrom(src => true));

            CreateMap<User, AddAdminViewModel>();

        }
    }
}
