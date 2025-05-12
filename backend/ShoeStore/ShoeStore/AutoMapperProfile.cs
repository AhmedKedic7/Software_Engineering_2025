using AutoMapper;
using ShoeStore.Contracts.Models;
using ShoeStore.Repository.Model;
namespace ShoeStoreAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, ProductContract>()
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.ColorName, opt => opt.MapFrom(src => src.Color.Name));

            CreateMap<ProductContract, Product>();
            CreateMap<User, UserContract>().ReverseMap();
            CreateMap<Order, OrderContract>().ReverseMap();
            CreateMap<Address, AddressContract>().ReverseMap();
            CreateMap<OrderItem, OrderItemContract>().ReverseMap();
            CreateMap<Color, ColorContract>().ReverseMap();
            CreateMap<ProductVersion, ProductVersionContract>().ReverseMap();
            CreateMap<Cart, CartContract>().ReverseMap();
            CreateMap<CartItem, CartItemContract>().ReverseMap();
            CreateMap<Image, ImageContract>().ReverseMap();
            CreateMap<Brand, BrandContract>().ReverseMap();
        }
    }
}
