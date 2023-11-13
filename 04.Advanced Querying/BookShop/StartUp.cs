namespace BookShop
{
    using BookShop.Models;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ValueGeneration;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            //string input = Console.ReadLine()!;

            //int result = RemoveBooks(db);
            //Console.WriteLine(result);

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


        //TASK-9.Book Search
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, books);
        }


        //TASK-10.Book Search by Author
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    FullName = $"{b.Author.FirstName} {b.Author.LastName}",
                    b.Title
                })
                .AsNoTracking()
                .ToArray();

            return string.Join(Environment.NewLine, books.Select(b => $"{b.Title} ({b.FullName})"));
        }


        //TASK-11.Count Books
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books
                .Where(b => b.Title.Length > lengthCheck)
                .AsNoTracking()
                .ToArray();

            return books.Count();
        }


        //TASK-12.Total Book Copies
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var books = context.Authors
                .Select(a => new
                {
                    FullName = $"{a.FirstName} {a.LastName}",
                    allCopies = a.Books.Sum(b => b.Copies)

                })
                .OrderByDescending(a => a.allCopies)
                .AsNoTracking()
                .ToArray();

            return string.Join(Environment.NewLine, books.Select(b => $"{b.FullName} - {b.allCopies}"));
        }


        //TASK-13.Profit by Category
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var books = context.Categories
                .Select(b => new
                {
                    categoryName = b.Name,
                    TotalProfit = b.CategoryBooks.Sum(b => b.Book.Copies * b.Book.Price)
                })
                .OrderByDescending(b => b.TotalProfit)
                .ThenBy(b => b.categoryName)
                .AsNoTracking()
                .ToArray();

            return string.Join(Environment.NewLine, books.Select(b => $"{b.categoryName} ${b.TotalProfit:f2}"));
        }


        //TASK-14.Most Recent Books
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var books = context.Categories
                .Select(b => new
                {
                    categotyName = b.Name,

                    booksByCateg = b.CategoryBooks
                    .OrderByDescending(b => b.Book.ReleaseDate)
                    .Select(cb => new
                    {
                        cb.Book.Title,
                        Years = cb.Book.ReleaseDate!.Value.Year
                    })
                    .Take(3)
                    .ToArray()
                })
                .OrderBy(b => b.categotyName)
                .AsNoTracking()
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var book in books)
            {
                sb.AppendLine($"--{book.categotyName}");

                foreach (var cate in book.booksByCateg)
                {
                    sb.AppendLine($"{cate.Title} ({cate.Years})");
                }
            }

            return sb.ToString().TrimEnd();
        }


        //TASK-15.Increase Prices
        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.ReleaseDate!.Value.Year < 2010);

            foreach (var book in books)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }


        //TASK-16.Remove Books
        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(b => b.Copies < 4200)
                .ToArray();


            foreach (var book in books)
            {
                context.Remove(book);
            }

            context.SaveChanges();

            return books.Count();
        }
    }
}