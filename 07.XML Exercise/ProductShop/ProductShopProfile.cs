using AutoMapper;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            //Users
            this.CreateMap<ImportUsersDto, User>();

            //Products 
            this.CreateMap<ImportProductDto, Product>();

            //Categories
            this.CreateMap<ImportCategoryDto, Category>();

            //CategoriesProducts
            this.CreateMap<ImportCategoriesProducts, CategoryProduct>();


            //Export CategoriesProduct
            this.CreateMap<Product, ExportProductsDto>()
                .ForMember(s => s.buyer, opt => opt.MapFrom(d => $"{d.Buyer.FirstName} {d.Buyer.LastName}"));


            //Export UsersDto
            //this.CreateMap<User, ExportUsersDto>()
            //    .ForMember(d => d.ProductSold, opt => opt.MapFrom(d => d.ProductsSold));

            //Export Categories 
            this.CreateMap<Category, ExportCategoryDto>()
                .ForMember(s => s.AveragePrice, opt => opt.MapFrom(d => d.CategoryProducts.Average(c => c.Product.Price)))
                .ForMember(s => s.TotalRevenue, opt => opt.MapFrom(d => d.CategoryProducts.Sum(c => c.Product.Price)))
                .ForMember(s => s.Count, opt => opt.MapFrom(d => d.CategoryProducts.Count));


            //Export UsersWhithProducts
            this.CreateMap<User, ExportUsersCountDto>();

            this.CreateMap<User, ExportUserWhithProductsDto>()
                .ForMember(s => s.SoldProducts, opt => opt.MapFrom(d => d.ProductsSold));

            //Export nested product 
            this.CreateMap<User, ExportUserWhithProductsDto>()
                .IgnoreAllPropertiesWithAnInaccessibleSetter();


            this.CreateMap<Product, ExportProductWhithNameAndPrice>();
        }
    }
}
