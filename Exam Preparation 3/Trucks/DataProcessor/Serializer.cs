namespace Trucks.DataProcessor
{
    using Data;
    using Newtonsoft.Json;
    using ProductShop.Utilities;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            var xmlHelper = new XmlHelper();

            var despatchers = context.Despatchers
                .ToArray()
                .Where(d => d.Trucks.Count >= 1)
                .Select(d => new ExportDespatchersDto
                {
                    Count = d.Trucks.Count,
                    DespatcherName = d.Name,

                    Trucks = d.Trucks
                                .Select(t => new ExportTrucks
                                {
                                    RegistrationNumber = t.RegistrationNumber,
                                    Make = t.MakeType
                                })
                                .OrderBy(t => t.RegistrationNumber)
                                .ToArray()
                })
                .OrderByDescending(d => d.Trucks.Count())
                .ThenBy(d => d.DespatcherName)
                .ToArray();

            return xmlHelper.Serialize(despatchers, "Despatchers");
        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            var clients = context.Clients
                .Where(c => c.ClientsTrucks.Any(ct => ct.Truck.TankCapacity >= capacity))
                .ToArray()
                .Select(c => new
                {
                    Name = c.Name,

                    Trucks = c.ClientsTrucks
                                .Where(ct => ct.Truck.TankCapacity >= capacity)
                                .Select(ct => new
                                {
                                    TruckRegistrationNumber = ct.Truck.RegistrationNumber,
                                    VinNumber = ct.Truck.VinNumber,
                                    TankCapacity = ct.Truck.TankCapacity,
                                    CargoCapacity = ct.Truck.CargoCapacity,
                                    CategoryType = ct.Truck.CategoryType.ToString(),
                                    MakeType = ct.Truck.MakeType.ToString()
                                })
                                .OrderBy(ct => ct.MakeType)
                                .ThenByDescending(ct => ct.CargoCapacity)
                                .ToArray()

                })
                .OrderByDescending(c => c.Trucks.Count())
                .ThenBy(c => c.Name)
                .Take(10)
                .ToArray();

            return JsonConvert.SerializeObject(clients,Formatting.Indented);
        }
    }
}
