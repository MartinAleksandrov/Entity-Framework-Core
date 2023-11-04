namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ValueGeneration;
    using System.Globalization;
    using System.Net.Http.Headers;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            string input = Console.ReadLine()!;

            string result = GetAuthorNamesEndingIn(db, input);
            Console.WriteLine(result);

        }

        //TASK-2.Age Restriction
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            bool isParsed = Enum.TryParse(typeof(AgeRestriction), command, true, out object? ageRestrictionobj);

            AgeRestriction ageRestriction;
            if (isParsed)
            {
                ageRestriction = (AgeRestriction)ageRestrictionobj!;

                var bookTitles = context.Books
                .Where(b => b.AgeRestriction == ageRestriction)
                .Select(b => b.Title)
                .OrderBy(b => b)
                .ToArray();

                return string.Join(Environment.NewLine, bookTitles);
            }

            return "Sorka";
        }


        //TASK-3.Golden Books
        public static string GetGoldenBooks(BookShopContext context)
        {
            bool isParsed = Enum.TryParse(typeof(EditionType), "Gold", true, out object? result);

            EditionType editionType;
            if (isParsed)
            {
                editionType = (EditionType)result!;

                var books = context.Books
                                   .Where(b => b.EditionType == editionType && b.Copies < 5000)
                                   .OrderBy(b => b.BookId)
                                   .Select(b => b.Title)
                                   .ToArray();

                return string.Join(Environment.NewLine, books);
            }
            return null!;
        }


        //TASK-4.Books by Price
        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Price > 40)
                .AsNoTracking()
                .OrderByDescending(b => b.Price)
                .Select(b => new
                {
                    b.Title,
                    Price = b.Price.ToString("f2")

                }).ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price}");
            }

            return sb.ToString().TrimEnd();
        }


        //TASK-5.Not Released In
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate!.Value.Year != year)
                .AsNoTracking()
                .OrderBy(b => b.BookId)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }


        //TASK-6.Book Titles by Category
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            StringBuilder sb = new StringBuilder();

            var books = context.Books
                .Where(b => b.BookCategories.Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                .AsNoTracking()
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }


        //TASK-7.Released Before Date
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime dt = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var books = context.Books
                .Where(b => b.ReleaseDate < dt)
                .AsNoTracking()
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    Price = b.Price.ToString("f2")
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType.ToString()} - ${book.Price}");
            }

            return sb.ToString().TrimEnd();
        }


        //TASK-8.Author Search
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(a => a.FirstName.EndsWith(input))
                .AsNoTracking()
                .Select(a => new
                {
                    FullName = a.FirstName + " " + a.LastName
                })
                .OrderBy(a => a.FullName)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var author in authors)
            {
                sb.AppendLine(author.FullName);
            }
            return sb.ToString().TrimEnd();
        }
    }
}


