using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using ProductShop.Utilities;
using System.Xml;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext productShopContext = new ProductShopContext();

            //var inputXML = File.ReadAllText(@"../../../Datasets/categories-products.xml");

            string result = GetCategoriesByProductsCount(productShopContext);
            Console.WriteLine(result);
        }

        //Create and return automapper
        public static IMapper CreateMapper()
        {
            var mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }));

            return mapper;
        }

        //1. Import Users
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            //Invoke method to create mapper
            IMapper mapper = CreateMapper();

            XmlHelper xmlHelper = new XmlHelper();

            var importDtos = xmlHelper.Deserialize<ImportUsersDto[]>(inputXml, "Users");

            var users = new HashSet<User>();


            //Map object from ImportUserDto to User
            foreach (var user in importDtos)
            {
                //Add this object to List
                users.Add(mapper.Map<User>(user));
            }

            //Add whole list to database
            context.Users.AddRange(users);

            //Save all changes
            context.SaveChanges();

            //Return output
            return $"Successfully imported {users.Count}";
        }


        //2. Import Products
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            XmlHelper xmlHelper = new XmlHelper();

            var xmlDtos = xmlHelper.Deserialize<ImportProductDto[]>(inputXml, "Products");

            var validProducts = new HashSet<Product>();

            foreach (var product in xmlDtos)
            {
                if (string.IsNullOrEmpty(product.Name))
                {
                    continue;
                }

                var mappedProduct = mapper.Map<Product>(product);
                validProducts.Add(mappedProduct);
            }

            context.Products.AddRange(validProducts);
            context.SaveChanges();

            return $"Successfully imported {validProducts.Count}";
        }


        //3. Import Categories
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var mapper = CreateMapper();

            XmlHelper xmlHelper = new XmlHelper();

            var categoryDtos = xmlHelper.Deserialize<ImportCategoryDto[]>(inputXml, "Categories");

            var validCategories = new HashSet<Category>();

            foreach (var category in categoryDtos)
            {
                if (string.IsNullOrEmpty(category.Name))
                {
                    continue;
                }

                var mappedCategory = mapper.Map<Category>(category);
                validCategories.Add(mappedCategory);
            }
            context.Categories.AddRange(validCategories);
            context.SaveChanges();

            return $"Successfully imported {validCategories.Count}";
        }


        //4. Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var mapper = CreateMapper();

            XmlHelper xmlHelper = new XmlHelper();

            var categoryProducts = xmlHelper.Deserialize<ImportCategoriesProducts[]>(inputXml, "CategoryProducts");

            var validCategories = new HashSet<CategoryProduct>();

            foreach (var category in categoryProducts)
            {
                if (!context.Categories.Any(c => c.Id == category.CategoryId) ||
                   !context.Products.Any(p => p.Id == category.ProductId))
                {
                    continue;
                }

                var mappedCategoriesProducts = mapper.Map<CategoryProduct>(category);
                validCategories.Add(mappedCategoriesProducts);
            }

            context.CategoryProducts.AddRange(validCategories);
            context.SaveChanges();

            return $"Successfully imported {validCategories.Count}";
        }


        //5. Export Products In Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();

            XmlHelper xmlHelper = new XmlHelper();

            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Take(10)
                .ProjectTo<ExportProductsDto>(mapper.ConfigurationProvider)
                .ToArray();

            return xmlHelper.Serialize(products, "Products");
        }


        //6. Export Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();

            var soldProducts = context
            .Users
            .AsNoTracking()
            .Where(u => u.ProductsSold.Any())
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .Take(5)
            .ProjectTo<ExportUsersDto>(mapper.ConfigurationProvider)
            .ToArray();

            return new XmlHelper()
                .Serialize(soldProducts, "Users");
        }


        //7. Export Categories By Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();

            XmlHelper xmlHelper = new XmlHelper();


            var categories = context.Categories
                .AsNoTracking()
                .ProjectTo<ExportCategoryDto>(mapper.ConfigurationProvider)
                .OrderByDescending(c => c.Count)
                .ThenBy(c => c.TotalRevenue)
                .ToArray();

            return xmlHelper.Serialize(categories, "Categories");
        }
    }
}