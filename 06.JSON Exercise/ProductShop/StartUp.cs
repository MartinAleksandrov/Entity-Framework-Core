using AutoMapper;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.DTOs.Import;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();

            //string inputJson = File.ReadAllText(@"../../../Datasets/categories-products.json");

            string result = GetUsersWithProducts(context);
            Console.WriteLine(result);
        }

        //Create Mapper
        public static IMapper CreateMap()
        {
            return new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }));
        }

        //Task-1
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var userDtos = JsonConvert.DeserializeObject<importUserDto[]>(inputJson);

            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }));

            ICollection<User> validUsers = new HashSet<User>();
            foreach (var u in userDtos!)
            {
                User user = mapper.Map<User>(u);

                validUsers.Add(user);
            }

            context.Users.AddRange(validUsers);
            context.SaveChanges();

            return $"Successfully imported {validUsers.Count}";
        }


        //Task-2
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var productDtos = JsonConvert.DeserializeObject<ImportProductDto[]>(inputJson);

            IMapper mapper = CreateMap();

            Product[] products = mapper.Map<Product[]>(productDtos);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }


        //Task-3
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMap();

            var categoriesDtos = JsonConvert.DeserializeObject<Category[]>(inputJson);

            List<Category> validCategories = new List<Category>();

            foreach (var catDto in categoriesDtos!)
            {
                if (catDto.Name.IsNullOrEmpty())
                {
                    continue;
                }

                validCategories.Add(catDto);
            }

            context.Categories.AddRange(validCategories);
            context.SaveChanges();

            return $"Successfully imported {validCategories.Count}";

        }


        //Task-4
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMap();

            var categProductsDtos = JsonConvert.DeserializeObject<ImportCategoryProductsDto[]>(inputJson);

            ICollection<CategoryProduct> validCategoryProducts = new HashSet<CategoryProduct>();

            foreach (var cp in categProductsDtos!)
            {
                //if (!context.Categories.Any(c => c.Id == cp.CategoryId) &&
                //    !context.Products.Any(p => p.Id == cp.ProductId))

                //{
                //    continue;
                //}

                CategoryProduct categoryProduct = mapper.Map<CategoryProduct>(cp);
                validCategoryProducts.Add(categoryProduct);
            }

            context.CategoriesProducts.AddRange(validCategoryProducts);
            context.SaveChanges();

            return $"Successfully imported {validCategoryProducts.Count}";
        }


        //Task-5
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    seller = p.Seller.FirstName + " " + p.Seller.LastName
                })
                .OrderBy(p => p.price)
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(products, Formatting.Indented);

        }


        //06. Export Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            IContractResolver contractResolver = ConfigureCamelCaseNaming();

            var products = context.Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    SoldProducts = u.ProductsSold
                        .Where(p => p.Buyer != null)
                        .Select(p => new
                        {
                            p.Name,
                            p.Price,
                            BuyerFirstName = p.Buyer!.FirstName,
                            BuyerLastName = p.Buyer.LastName
                        })
                        .ToArray()
                })
                .AsNoTracking()
                .ToArray();


            return JsonConvert.SerializeObject(products,
                Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ContractResolver = contractResolver
                });
        }


        //07. Export Categories By Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            IContractResolver contractResolver = ConfigureCamelCaseNaming();

            var categByProductsCount = context.Categories
                .OrderByDescending(c => c.CategoriesProducts.Count)
                .Select(c => new
                {
                    Category = c.Name,
                    ProductsCount = c.CategoriesProducts.Count,
                    AveragePrice = c.CategoriesProducts.Average(c => c.Product.Price).ToString("f2"),
                    TotalRevenue = c.CategoriesProducts.Sum(c => c.Product.Price).ToString("f2")
                })
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(categByProductsCount,
              Formatting.Indented,
              new JsonSerializerSettings()
              {
                  ContractResolver = contractResolver
              });
        }

        //08. Export Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            IContractResolver contractResolver = ConfigureCamelCaseNaming();

            var users = context
                .Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .Select(u => new
                {
                    // UserDTO
                    u.FirstName,
                    u.LastName,
                    u.Age,
                    SoldProducts = new
                    {
                        // ProductWrapperDTO
                        Count = u.ProductsSold
                            .Count(p => p.Buyer != null),
                        Products = u.ProductsSold
                            .Where(p => p.Buyer != null)
                            .Select(p => new
                            {
                                // ProductDTO
                                p.Name,
                                p.Price
                            })
                            .ToArray()
                    }
                })
                .OrderByDescending(u => u.SoldProducts.Count)
                .AsNoTracking()
                .ToArray();

            var userWrapperDto = new
            {
                UsersCount = users.Length,
                Users = users
            };

            return JsonConvert.SerializeObject(userWrapperDto,
                Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ContractResolver = contractResolver,
                    NullValueHandling = NullValueHandling.Ignore
                });
        }

        private static IContractResolver ConfigureCamelCaseNaming()
        {
            return new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy(false, true)
            };
        }
    }
}