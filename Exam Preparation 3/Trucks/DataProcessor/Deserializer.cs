namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Security.Cryptography;
    using System.Text;
    using Data;
    using Newtonsoft.Json;
    using ProductShop.Utilities;
    using Trucks.Data.Models;
    using Trucks.Data.Models.Enums;
    using Trucks.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            var xmlHelper = new XmlHelper();
            var sb = new StringBuilder();

            var despathcersDtos = xmlHelper.Deserialize<ImportDespaherDto[]>(xmlString, "Despatchers");

            var validDespathcers = new List<Despatcher>();

            foreach (var despatcherDTO in despathcersDtos)
            {
                if (!IsValid(despatcherDTO)
                    || string.IsNullOrEmpty(despatcherDTO.Name)
                    || string.IsNullOrEmpty(despatcherDTO.Position))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var despatcher = new Despatcher()
                {
                    Name = despatcherDTO.Name,
                    Position = despatcherDTO.Position
                };

                foreach (var truckDto in despatcherDTO.Trucks)
                {
                    if (!IsValid(truckDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var truck = new Truck()
                    {
                        RegistrationNumber = truckDto.RegistrationNumber,
                        VinNumber = truckDto.VinNumber,
                        TankCapacity = truckDto.TankCapacity,
                        CargoCapacity = truckDto.CargoCapacity,
                        CategoryType = (CategoryType)truckDto.CategoryType,
                        MakeType = (MakeType)truckDto.MakeType
                    };

                    despatcher.Trucks.Add(truck);
                }

                sb.AppendLine(string.Format(SuccessfullyImportedDespatcher,despatcher.Name,despatcher.Trucks.Count));
                validDespathcers.Add(despatcher);
            }

            context.Despatchers.AddRange(validDespathcers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var clientsJson = JsonConvert.DeserializeObject<ImportClientsDto[]>(jsonString);

            var validClients = new HashSet<Client>();

            foreach (var clientDto in clientsJson)
            {
                if (!IsValid(clientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (clientDto.Type == "usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var client = new Client()
                {
                    Name = clientDto.Name,
                    Nationality = clientDto.Nationality,
                    Type = clientDto.Type
                };


                foreach (var truckId in clientDto.Trucks.Distinct())
                {
                    if (!context.Trucks.Any(t => t.Id == truckId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var truck = new ClientTruck()
                    {
                        TruckId = truckId
                    };

                    client.ClientsTrucks.Add(truck);
                }

                validClients.Add(client);
                sb.AppendLine(String.Format(SuccessfullyImportedClient,client.Name,client.ClientsTrucks.Count));
            }

            context.AddRange(validClients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}