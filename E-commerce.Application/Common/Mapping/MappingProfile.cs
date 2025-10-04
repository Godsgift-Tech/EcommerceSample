using AutoMapper;
using E_commerce.Application.Common.ServiceImplementations.ServiceDTO.Order;
using E_commerce.Application.Common.ServiceImplementations.ServiceDTO.Payment;
using E_commerce.Application.Common.ServiceImplementations.ServiceDTO.Product;
using E_commerce.Application.Common.ServiceImplementations.ServiceDTO.ProductCategory;
using E_commerce.Core.Entities;

namespace E_commerce.Application.Common.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // -----------------------------
            // CATEGORY
            // -----------------------------
            CreateMap<CreateCategoryDto, Category>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
            CreateMap<Category, CreateCategoryDto>();

            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, GetCategoryDto>().ReverseMap();

            CreateMap<UpdateCategoryDto, Category>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
            CreateMap<Category, UpdateCategoryDto>();

            // -----------------------------
            // PRODUCT
            // -----------------------------
            CreateMap<CreateProductDto, Product>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
            CreateMap<Product, CreateProductDto>();

            CreateMap<Product, GetProductDto>()
                .ForMember(dest => dest.DisplayPrice,
                           opt => opt.MapFrom(src => $"{src.Currency} {src.UnitPrice:N2}"));

            CreateMap<Product, ProductSummaryDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();

            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
            CreateMap<Product, UpdateProductDto>();

            // -----------------------------
            // ORDER
            // -----------------------------
            // CreateOrderDto → Order (items built manually in service)
            CreateMap<CreateOrderDto, Order>()
                .ForMember(dest => dest.Items, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            // UpdateOrderDto → Order
            CreateMap<UpdateOrderDto, Order>()
              .ForMember(dest => dest.Items, opt => opt.Ignore());

            // OrderItem → OrderProductDto
            CreateMap<OrderItem, OrderProductDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity)) 
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Product.Currency))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Quantity * src.UnitPrice));

            // Order → OrderDto (map Items → Products)
            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Items));

            // Order → GetOrderDto
            CreateMap<Order, GetOrderDto>()
                .ForMember(dest => dest.Products, opt => opt.MapFrom(src => src.Items));

            // PAYMENT
            // -----------------------------
            CreateMap<CreatePaymentDto, Payment>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.PaymentDate, opt => opt.Ignore())
                .ForMember(dest => dest.TransactionReference, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore());

            CreateMap<UpdatePaymentDto, Payment>()
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            // Map Payment → GetPaymentDto (include OrderAmount from related Order)
            CreateMap<Payment, GetPaymentDto>()
                .ForMember(dest => dest.OrderAmount,
                    opt => opt.MapFrom(src => src.Order != null ? src.Order.OrderAmount : 0))
                .ForMember(dest => dest.TransactionReference, opt => opt.MapFrom(src => src.TransactionReference))
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            // Optional: if needed for admin or all-payment retrieval
            CreateMap<Payment, PaymentDto>().ReverseMap();
        }
    }
}
