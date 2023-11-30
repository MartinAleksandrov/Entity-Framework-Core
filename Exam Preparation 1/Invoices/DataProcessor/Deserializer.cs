namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using System.Xml;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.DataProcessor.ImportDto;
    using Newtonsoft.Json;
    using ProductShop.Utilities;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";

        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            var sb = new StringBuilder();
            var xmlHelper = new XmlHelper();

            var clientDtos = xmlHelper.Deserialize<ImportClientsDto[]>(xmlString, "Clients");

            ICollection<Client> validClients = new HashSet<Client>();

            foreach (var clientDto in clientDtos)
            {
                if (!IsValid(clientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                ICollection<Address> validAddresses = new HashSet<Address>();

                foreach (var addressDto in clientDto.Addresses)
                {
                    if (!IsValid(addressDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var address = new Address
                    {
                        StreetName = addressDto.StreetName,
                        StreetNumber = addressDto.StreetNumber,
                        PostCode = addressDto.PostCode,
                        City = addressDto.City,
                        Country = addressDto.Country
                    };

                    validAddresses.Add(address);
                }

                var client = new Client
                {
                    Name = clientDto.Name,
                    NumberVat = clientDto.NumberVat,
                    Addresses = validAddresses
                };

                validClients.Add(client);
                sb.AppendLine(string.Format(SuccessfullyImportedClients, client.Name));
            }

            context.Clients.AddRange(validClients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            var jsonDtos = JsonConvert.DeserializeObject<ImportInvoicesDto[]>(jsonString);

            var validInvoices = new HashSet<Invoice>();

            var sb = new StringBuilder();

            foreach (var invoiceDto in jsonDtos)
            {
                if (!IsValid(invoiceDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (invoiceDto.DueDate == DateTime.ParseExact("01/01/0001", "dd/MM/yyyy", CultureInfo.InvariantCulture) || invoiceDto.IssueDate == DateTime.ParseExact("01/01/0001", "dd/MM/yyyy", CultureInfo.InvariantCulture))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                Invoice invoice = new Invoice()
                {
                    Number = invoiceDto.Number,
                    IssueDate = invoiceDto.IssueDate,
                    DueDate = invoiceDto.DueDate,
                    CurrencyType = invoiceDto.CurrencyType,
                    Amount = invoiceDto.Amount,
                    ClientId = invoiceDto.ClientId
                };

                if (invoice.IssueDate > invoice.DueDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                validInvoices.Add(invoice);

                sb.AppendLine(String.Format(SuccessfullyImportedInvoices, invoice.Number));
            }
            context.Invoices.AddRange(validInvoices);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {
            var productsDto = JsonConvert.DeserializeObject<ImportProductDto[]>(jsonString);

            var validProducts = new HashSet<Product>();
            var sb = new StringBuilder();

            foreach (var productDto in productsDto)
            {
                if (!IsValid(productDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var product = new Product()
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    CategoryType = productDto.CategoryType,
                };

                foreach (var Id in productDto.Clients.Distinct())
                {
                    var client = context.Clients.FirstOrDefault(c => c.Id == Id);
                    if (client == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var pk = new ProductClient
                    {
                        Client = client
                    };
                    product.ProductsClients.Add(pk);
                }
                sb.AppendLine(string.Format(SuccessfullyImportedProducts, product.Name, product.ProductsClients.Count()));
                validProducts.Add(product);
            }

            context.AddRange(validProducts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
