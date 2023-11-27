namespace Footballers.DataProcessor
{
    using Footballers.Data;
    using Footballers.Data.Models;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ImportDto;
    using Microsoft.VisualBasic;
    using Newtonsoft.Json;
    using ProductShop.Utilities;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            var xmlHelper = new XmlHelper();

            var sb = new StringBuilder();

            var coachesDto = xmlHelper.Deserialize<ImportCoachesDto[]>(xmlString, "Coaches");

            var validCoaches = new HashSet<Coach>();

            foreach (var coachDto in coachesDto)
            {
                if (!IsValid(coachDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                string nationality = coachDto.Nationality;
                bool isNationalityInvalid = string.IsNullOrEmpty(nationality);

                if (isNationalityInvalid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var coach = new Coach()
                {
                    Name = coachDto.Name,
                    Nationality = coachDto.Nationality
                };

                foreach (var footballerDto in coachDto.Footballers)
                {
                    if (!IsValid(footballerDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isStartDateValid = DateTime.TryParseExact(footballerDto.ContractStartDate, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime startDate);
                    if (!isStartDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool isEndDateValid = DateTime.TryParseExact(footballerDto.ContractEndDate, "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime endDate);
                    if (!isEndDateValid)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (startDate >= endDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var footballer = new Footballer()
                        {
                            Name = footballerDto.Name,
                            ContractStartDate = startDate,
                            ContractEndDate =endDate,
                            BestSkillType = (BestSkillType)footballerDto.BestSkillType,
                            PositionType = (PositionType)footballerDto.PositionType
                        };
                        coach.Footballers.Add(footballer);
                   
                }
                validCoaches.Add(coach);
                sb.AppendLine(string.Format(SuccessfullyImportedCoach, coach.Name, coach.Footballers.Count));
            }

            context.AddRange(validCoaches);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            var teamsJson = JsonConvert.DeserializeObject<ImportTeamsDto[]>(jsonString);

            var sb = new StringBuilder();

            var validTeams = new HashSet<Team>();

            foreach (var teamDto in teamsJson)
            {
                if (!IsValid(teamDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (string.IsNullOrEmpty(teamDto.Nationality) || teamDto.Trophies == 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Team team = new Team()
                {
                    Name = teamDto.Name,
                    Nationality = teamDto.Nationality,
                    Trophies = teamDto.Trophies
                };

                foreach (var fId in teamDto.Footballers.Distinct())
                {
                    if (!context.Footballers.Any(f => f.Id == fId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var footballer = new TeamFootballer()
                    {
                        FootballerId = fId
                    };
                    team.TeamsFootballers.Add(footballer);
                }
                validTeams.Add(team);
                sb.AppendLine(string.Format(SuccessfullyImportedTeam,team.Name,team.TeamsFootballers.Count));
            }
            context.AddRange(validTeams);
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
