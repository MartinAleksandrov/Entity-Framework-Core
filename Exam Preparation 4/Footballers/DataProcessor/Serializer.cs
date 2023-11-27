namespace Footballers.DataProcessor
{
    using Data;
    using Footballers.Data.Models.Enums;
    using Footballers.DataProcessor.ExportDto;
    using Microsoft.EntityFrameworkCore.ValueGeneration;
    using Newtonsoft.Json;
    using ProductShop.Utilities;
    using System.Globalization;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            var xmlHelper = new XmlHelper();

            var coaches = context.Coaches
                .Where(c => c.Footballers.Count >= 1)
                .ToArray()
                .Select(c => new ExportCoachesDto
                {
                    Count = c.Footballers.Count,
                    CoachName = c.Name,

                    Footballers = c.Footballers
                                    .Select(f => new ExportFootbllers
                                    {
                                        Name = f.Name,
                                        Position = f.PositionType.ToString()
                                    })
                                    .OrderBy(f => f.Name)
                                    .ToArray()
                })
                .OrderByDescending(c => c.Footballers.Count())
                .ThenBy(c => c.CoachName)
                .ToArray();

            return xmlHelper.Serialize(coaches, "Coaches");
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var teams = context.Teams
                .Where(t => t.TeamsFootballers.Any(tf => tf.Footballer.ContractStartDate >= date))
                .ToArray()
                .Select(t => new
                {
                    t.Name,

                    Footballers = t.TeamsFootballers
                                    .Where(tf => tf.Footballer.ContractStartDate >= date)
                                    .ToArray()
                                    .OrderByDescending(tf => tf.Footballer.ContractEndDate)
                                    .ThenBy(tf => tf.Footballer.Name)
                                    .Select(tf => new
                                    {
                                        FootballerName = tf.Footballer.Name,
                                        ContractStartDate = tf.Footballer.ContractStartDate.ToString("d", CultureInfo.InvariantCulture),
                                        ContractEndDate = tf.Footballer.ContractEndDate.ToString("d", CultureInfo.InvariantCulture),
                                        BestSkillType = tf.Footballer.BestSkillType.ToString(),
                                        PositionType = tf.Footballer.PositionType.ToString()
                                    })
                                    .ToArray()
                })
                .OrderByDescending(t => t.Footballers.Length)
                .ThenBy(t => t.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(teams, Formatting.Indented);
        }
    }
}
