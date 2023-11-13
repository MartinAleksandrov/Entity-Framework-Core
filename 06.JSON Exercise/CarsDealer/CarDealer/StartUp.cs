using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext carDealerContext = new CarDealerContext();

            //var inputJson = File.ReadAllText(@"../../../Datasets/sales.json");

            string result = GetSalesWithAppliedDiscount(carDealerContext);
            Console.WriteLine(result);
        }

        public static IMapper CreateMapper()
        {
            return new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));
        }


        //01. Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var suppliers = JsonConvert.DeserializeObject<ImportSuppliersDto[]>(inputJson);

            IMapper mapper = CreateMapper();

            var mappedSuppliers = mapper.Map<Supplier[]>(suppliers);

            context.Suppliers.AddRange(mappedSuppliers);
            context.SaveChanges();

            return $"Successfully imported {mappedSuppliers.Length}.";
        }


        //02. Import Parts
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            var partsJsonDtos = JsonConvert.DeserializeObject<ImportPartsDto[]>(inputJson);

            var validParts = new HashSet<Part>();
            foreach (var part in partsJsonDtos!)
            {
                if (!context.Suppliers.Any(s => s.Id == part.SupplierId))
                {
                    continue;
                }

                var mapped = mapper.Map<Part>(part);
                validParts.Add(mapped);
            }

            context.Parts.AddRange(validParts);
            context.SaveChanges();

            return $"Successfully imported {validParts.Count}.";
        }


        //03. Import Cars
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            var jsonCarsDto = JsonConvert.DeserializeObject<ImportCarsDto[]>(inputJson);


            var validCars = new List<Car>();

            foreach (var car in jsonCarsDto!)
            {
                var cars = mapper.Map<Car>(car);
                validCars.Add(cars);
            }

            context.Cars.AddRange(validCars);
            context.SaveChanges();

            return $"Successfully imported {validCars.Count}.";
        }


        //04. Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            var jsonCustomers = JsonConvert.DeserializeObject<ImportCustomerDto[]>(inputJson);

            var validCustomers = new HashSet<Customer>();


            foreach (var cust in jsonCustomers!)
            {
                if (context.Customers.Any(c => c.Name == cust.Name))
                {
                    continue;
                }
                var mappedCustomer = mapper.Map<Customer>(cust);
                validCustomers.Add(mappedCustomer);
            }

            context.Customers.AddRange(validCustomers);
            context.SaveChanges();

            return $"Successfully imported {validCustomers.Count}.";
        }


        //05. Import Sales
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            var salesJson = JsonConvert.DeserializeObject<ImportSalesDto[]>(inputJson);

            var mappedSales = mapper.Map<Sale[]>(salesJson);

            context.Sales.AddRange(mappedSales);
            context.SaveChanges();

            return $"Successfully imported {mappedSales.Length}.";
        }


        //06. Export Ordered Customers
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            var customerJson = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver ? 1 : 0)
                .Select(c => new
                {
                    c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                    c.IsYoungDriver
                })
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(customerJson, Formatting.Indented);
        }


        //07. Export Cars From Make Toyota
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var carsJson = context.Cars
             .Where(c => c.Make == "Toyota")
             .Select(c => new
             {
                 c.Id,
                 c.Make,
                 c.Model,
                 c.TraveledDistance
             })
             .AsNoTracking()
             .OrderBy(c => c.Model)
             .ThenByDescending(c => c.TraveledDistance)
             .ToArray();

            return JsonConvert.SerializeObject(carsJson, Formatting.Indented);
        }


        //08. Export Local Suppliers
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var suppliersJson = context.Suppliers
                .Where(s => !s.IsImporter)
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    PartsCount = s.Parts.Count
                })
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(suppliersJson, Formatting.Indented);
        }


        //09. Export Cars With Their List Of Parts
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carsWhithPartsJson = context.Cars
                .Select(c => new
                {
                    car = new
                    {
                        c.Make,
                        c.Model,
                        c.TraveledDistance
                    },

                    parts = c.PartsCars
                    .Select(p => new
                    {
                        p.Part.Name,
                        Price = p.Part.Price.ToString("f2")
                    })
                    .ToArray()

                })
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(carsWhithPartsJson, Formatting.Indented);
        }


        //10. Export Total Sales By Customer
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var customersJson = context.Customers
                .Where(c => c.Sales.Count >= 1)
                .Select(c => new
                {
                    fullName = c.Name,
                    boughtCars = c.Sales.Count,
                    spentMoney = c.Sales.SelectMany(s => s.Car.PartsCars).Sum(pc => pc.Part.Price)
                })
                .AsNoTracking()
                .OrderByDescending(c => c.spentMoney)
                .ThenByDescending(c => c.boughtCars)
                .ToArray();

            return JsonConvert.SerializeObject(customersJson, Formatting.Indented);
        }


        //11. Export Sales With Applied Discount
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var salesJson = context.Sales
                .Take(10)
                .Select(s => new
                {
                    car = new
                    {
                        s.Car.Make,
                        s.Car.Model,
                        s.Car.TraveledDistance
                    },

                    customerName = s.Customer.Name,
                    discount = s.Discount.ToString("f2"),
                    price = s.Car.PartsCars.Sum(pc => pc.Part.Price).ToString("f2"),
                    priceWithDiscount = (s.Car.PartsCars.Sum(pc => pc.Part.Price) * (1 - s.Discount / 100)).ToString("f2")
                })
                .AsNoTracking()
                .ToArray();

            return JsonConvert.SerializeObject(salesJson, Formatting.Indented);
        }
    }
}