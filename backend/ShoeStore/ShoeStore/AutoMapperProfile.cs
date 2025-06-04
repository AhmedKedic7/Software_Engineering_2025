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
            CreateMap<User, UserProfileContract>()
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
            .ForMember(dest => dest.Addresses,
                opt => opt.MapFrom(src => src.Addresses))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<Address, AddressContract>();
            CreateMap<User, UserContract>().ReverseMap();
            
            CreateMap<Address, AddressContract>().ReverseMap();
            CreateMap<OrderItem, OrderItemContract>().ReverseMap();
            CreateMap<Color, ColorContract>().ReverseMap();
            CreateMap<ProductVersion, ProductVersionContract>().ReverseMap();
            CreateMap<Cart, CartContract>().ReverseMap();
            CreateMap<CartItem, CartItemContract>().ReverseMap();
            CreateMap<Image, ImageContract>().ReverseMap();
            CreateMap<Brand, BrandContract>().ReverseMap();

            CreateMap<User, LoginResponse>()
                .ForMember(dest => dest.IsAdmin, opt => opt.MapFrom(src => src.IsAdmin == 1));
            
            CreateMap<Cart, CartContract>()
            .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems));

            CreateMap<CartItem, CartItemContract>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));

            CreateMap<Order, OrderContract>()
                .ForMember(dest => dest.ContactName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.User.PhoneNumber))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.Address.AddressLine))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            CreateMap<OrderContract, Order>();

            CreateMap<OrderItem, SoldProductContract>()
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.TotalSold, opt => opt.MapFrom(src => src.Quantity));

            CreateMap<OrderItem, OrderCheckoutContract>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.ImageURLs, opt => opt.MapFrom(src => src.Product.Images.Select(img => img.ImageUrl).ToList()));

            CreateMap<RegisterContract, User>();

            CreateMap<CreateProductContract.CreateImageContract, Image>()
            .ForMember(dest => dest.ImageId, opt => opt.Ignore())
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<UpdateProductContract, Product>()
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(i => new Image
            {
                ImageUrl = i.ImageUrl,
                IsMain = i.IsMain
             }).ToList()));
               }
    }
}
