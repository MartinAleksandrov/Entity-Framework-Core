namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using ProductShop.Utilities;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var xmlHepler = new XmlHelper();

            var countriesDtos = xmlHepler.Deserialize<ImportCountries[]>(xmlString, "Countries");

            var validCountries = new HashSet<Country>();

            foreach (var countriesDto in countriesDtos)
            {
                if (!IsValid(countriesDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var country = new Country()
                {
                    CountryName = countriesDto.CountryName,
                    ArmySize = countriesDto.ArmySize
                };

                validCountries.Add(country);
                sb.AppendLine(String.Format(SuccessfulImportCountry, country.CountryName, country.ArmySize));
            }

            context.Countries.AddRange(validCountries);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var xmlHepler = new XmlHelper();

            var manufacturersDtos = xmlHepler.Deserialize<ImportManufacturers[]>(xmlString, "Manufacturers");

            var validManufacturers = new HashSet<Manufacturer>();

            foreach (var manufacturerDto in manufacturersDtos)
            {
                if (!IsValid(manufacturerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var manufacturer = new Manufacturer()
                {
                    ManufacturerName = manufacturerDto.ManufacturerName,
                    Founded = manufacturerDto.Founded
                };

                validManufacturers.Add(manufacturer);
                sb.AppendLine(String.Format(SuccessfulImportManufacturer, manufacturer.ManufacturerName, manufacturer.Founded));
            }

            context.Manufacturers.AddRange(validManufacturers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var xmlHepler = new XmlHelper();

            var shellsDtos = xmlHepler.Deserialize<ImportShellDto[]>(xmlString, "Shells");

            var validShells = new HashSet<Shell>();

            foreach (var shellDto in shellsDtos)
            {
                if (!IsValid(shellDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var shell = new Shell()
                {
                    ShellWeight= shellDto.ShellWeight,
                    Caliber = shellDto.Caliber
                };

                validShells.Add(shell);
                sb.AppendLine(String.Format(SuccessfulImportShell, shell.Caliber, shell.ShellWeight));
            }

            context.Shells.AddRange(validShells);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var gunsJson = JsonConvert.DeserializeObject<ImportGuns[]>(jsonString);

            var validGuns = new HashSet<Gun>();
            var validGunTypes = new string[] { "Howitzer", "Mortar", "FieldGun", "AntiAircraftGun", "MountainGun", "AntiTankGun" };


            foreach (var gunJson in gunsJson)
            {
                if (!IsValid(gunJson) || !validGunTypes.Contains(gunJson.GunType))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var gun = new Gun()
                {
                    ManufacturerId = gunJson.ManufacturerId,
                    GunWeight = gunJson.GunWeight,
                    BarrelLength = gunJson.BarrelLength,
                    NumberBuild = gunJson.NumberBuild,
                    Range = gunJson.Range,
                    GunType = (GunType)Enum.Parse(typeof(GunType), gunJson.GunType),
                    ShellId = gunJson.ShellId
                };

                foreach (var countryIdDto in gunJson.Countries)
                {
                    var coountryGuns = new CountryGun()
                    {
                        CountryId = countryIdDto.Id
                    };

                    gun.CountriesGuns.Add(coountryGuns);
                }

                validGuns.Add(gun);
                sb.AppendLine(string.Format(SuccessfulImportGun, gun.GunType, gun.GunWeight, gun.BarrelLength));
            }

            context.Guns.AddRange(validGuns);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}