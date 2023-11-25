namespace Boardgames.DataProcessor
{
    using Boardgames.Data;
    using Boardgames.Data.Models;
    using Boardgames.DataProcessor.ExportDto;
    using Boardgames.Utilities;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using ProductShop.Utilities;
    using System.Xml;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            var xmlHelper = new XmlHelper();

            var creators = context.Creators
                .ToArray()
                .Select(c => new ExportCreatorsWithTheirBoardgames
                {
                    BoardgamesCount = c.Boardgames.Count,
                    CreatorName = $"{c.FirstName} {c.LastName}",

                    Boardgames = c.Boardgames
                                  .Select(b => new ExportBoardGames
                                  {

                                      BoardgameName = b.Name,
                                      BoardgameYearPublished = b.YearPublished

                                  })
                                  .OrderBy(b => b.BoardgameName)
                                  .ToArray()
                })
                .OrderByDescending(c => c.Boardgames.Count())
                .ThenBy(c => c.CreatorName)
                .ToArray();

            return xmlHelper.Serialize<ExportCreatorsWithTheirBoardgames[]>(creators, "Creators");
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellers = context.Sellers
                .Where(s => s.BoardgamesSellers
                .Any(bs => bs.Boardgame.YearPublished >= year
                  && bs.Boardgame.Rating <= rating))
                .AsNoTracking()
                .Select(s => new
                {
                    s.Name,
                    s.Website,
                    Boardgames = s.BoardgamesSellers
                    .Where(bs => bs.Boardgame.YearPublished >= year && bs.Boardgame.Rating <= rating)
                    .Select(bs => new
                    {
                        Name = bs.Boardgame.Name,
                        Rating = bs.Boardgame.Rating,
                        Mechanics = bs.Boardgame.Mechanics,
                        Category = bs.Boardgame.CategoryType.ToString()
                    })
                    .OrderByDescending(bs => bs.Rating)
                    .ThenBy(bs => bs.Name)
                    .ToArray()

                })
                .OrderByDescending(bs => bs.Boardgames.Count())
                .ThenBy(bs => bs.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(sellers, Formatting.Indented);
        }
    }
}