namespace Boardgames.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.Data.Models.Enums;
    using Boardgames.DataProcessor.ImportDto;
    using Newtonsoft.Json.Linq;
    using ProductShop.Utilities;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCreator
            = "Successfully imported creator – {0} {1} with {2} boardgames.";

        private const string SuccessfullyImportedSeller
            = "Successfully imported seller - {0} with {1} boardgames.";

        public static string ImportCreators(BoardgamesContext context, string xmlString)
        {
            var xmlHelper = new XmlHelper();//Initialize xmlhelper Instance 

            var sb = new StringBuilder();

            //Deserialize xmlString to proper Dto
            var creatorsDtos = xmlHelper.Deserialize<ImportCreatorsDto[]>(xmlString, "Creators");

            //Creating collection only for caliv instances
            var validCreators = new List<Creator>();

            foreach (var creatorDto in creatorsDtos)//Outer DTO
            {
                if (!IsValid(creatorDto))//Check if current creator is valid
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                //Creating new instance of creato and manual map his properties except collcetion prop
                var creator = new Creator()
                {
                    FirstName = creatorDto.FirstName,
                    LastName = creatorDto.LastName
                };

                //Iterate through collection of boardgames in outer DTO
                foreach (var boardDto in creatorDto.Boardgames)//Inner DTO
                {

                    if (!IsValid(boardDto))//Check if current boardgame is valid
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }


                    //Manual map all properites of inerDto 
                    var boardGame = new Boardgame()
                    {
                        Name = boardDto.Name,
                        Rating = boardDto.Rating,
                        YearPublished = boardDto.YearPublished,
                        CategoryType = (CategoryType)boardDto.CategoryType,
                        Mechanics = boardDto.Mechanics
                    };

                    creator.Boardgames.Add(boardGame);//Add created instance directly in outerDto Creator collection
                }

                //Finaly add creator to collection of valid creators
                validCreators.Add(creator);

                sb.AppendLine(string.Format(SuccessfullyImportedCreator,creator.FirstName,creator.LastName,creator.Boardgames.Count));
            }

            context.Creators.AddRange(validCreators);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportSellers(BoardgamesContext context, string jsonString)
        {
            throw new NotImplementedException();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
