using AutoMapper;
using CarDealer.Data;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using CarDealer.Utilities;
using System.Net.WebSockets;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            CarDealerContext context = new CarDealerContext();

            string inputXml = File.ReadAllText(@"../../../Datasets/sales.xml");

            string result = ImportSales(context, inputXml);
            Console.WriteLine(result);
        }

        //Create Mapper
        public static IMapper CreateMapper()
        {
            return new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));
        }

        //1. Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var mapper = CreateMapper();

            var xmlHelper = new XmlHelper();

            var supliers = xmlHelper.Deserialize<ImportSuppliersDto[]>("Suppliers", inputXml);

            var validSuppliers = new HashSet<Supplier>();

            foreach (var suplier in supliers)
            {
                var mappedSuplier = mapper.Map<Supplier>(suplier);
                validSuppliers.Add(mappedSuplier);
            }

            context.AddRange(validSuppliers);
            context.SaveChanges();

            return $"Successfully imported {validSuppliers.Count}";
        }


        //2. Import Parts
        //This task is solved without using auotmapper
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var xmlHelper = new XmlHelper();

            var parts = xmlHelper.Deserialize<ImportPartDto[]>("Parts", inputXml);

            var validParts = new HashSet<Part>();

            foreach (var part in parts)
            {
                if (!context.Suppliers.Any(p => p.Id == part.SupplierId))
                {
                    continue;
                }

                //Using manual mapping just for fun :D
                Part mappedPart = new Part()
                {
                    Name = part.Name,
                    Price = part.Price,
                    Quantity = part.Quantity,
                    SupplierId = part.SupplierId
                };

                validParts.Add(mappedPart);
            }

            context.AddRange(validParts);
            context.SaveChanges();

            return $"Successfully imported {validParts.Count}";
        }


        //3. Import Cars
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var mapper = CreateMapper();

            var xmlHelper = new XmlHelper();

            var carsDtos = xmlHelper.Deserialize<ImportCarsDto[]>("Cars", inputXml);

            var validCars = new HashSet<Car>();

            foreach (var carDto in carsDtos)
            {
                if (string.IsNullOrEmpty(carDto.Make) ||
                   string.IsNullOrEmpty(carDto.Model))
                {
                    continue;
                }

                Car car = mapper.Map<Car>(carDto);

                foreach (var partDto in carDto.Pasrts.DistinctBy(c => c.PartId))
                {
                    if (!context.Parts.Any(p => p.Id == partDto.PartId))
                    {
                        continue;
                    }

                    PartCar partCar = new PartCar()
                    {
                        PartId = partDto.PartId
                    };
                    car.PartsCars.Add(partCar);
                }

                validCars.Add(car);
            }

            context.AddRange(validCars);
            context.SaveChanges();

            return $"Successfully imported {validCars.Count}";
        }


        //4. Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var mapper = CreateMapper();

            var xmlHelper = new XmlHelper();

            var customers = xmlHelper.Deserialize<ImportCustomerDto[]>("Customers", inputXml);

            var validCustomers = new HashSet<Customer>();

            foreach (var customerDto in customers)
            {
                var mapppedCustomer = mapper.Map<Customer>(customerDto);
                validCustomers.Add(mapppedCustomer);
            }

            context.AddRange(validCustomers);
            context.SaveChanges();

            return $"Successfully imported {validCustomers.Count}";
        }

        //5. Import Sales
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();
            XmlHelper xmlHelper = new XmlHelper();

            ImportSaleDto[] saleDtos =
                xmlHelper.Deserialize<ImportSaleDto[]>("Sales", inputXml);

            // Optimization
            ICollection<int> dbCarIds = context.Cars
                .Select(c => c.Id)
                .ToArray();

            ICollection<Sale> validSales = new HashSet<Sale>();
            foreach (ImportSaleDto saleDto in saleDtos)
            {
                if (!saleDto.CarId.HasValue ||
                    dbCarIds.All(id => id != saleDto.CarId.Value))
                {
                    continue;
                }

                Sale sale = mapper.Map<Sale>(saleDto);
                validSales.Add(sale);
            }

            context.Sales.AddRange(validSales);
            context.SaveChanges();

            return $"Successfully imported {validSales.Count}";
        }
    }
}