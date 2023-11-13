namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Text;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here

            //string albumsInfoResult = ExportAlbumsInfo(context,9);
            //Console.WriteLine(albumsInforesult);

            string songsAboveDurationResult = ExportSongsAboveDuration(context, 4);
            Console.WriteLine(songsAboveDurationResult);
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albums = context.Albums
                .Where(a => a.ProducerId.HasValue && a.ProducerId == producerId)
                .ToArray()
                .OrderByDescending(a => a.Price)
                .Select(a => new
                {
                    AlbumName = a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProducerName = a.Producer!.Name,
                    Toral_Album_Price = a.Price.ToString("f2"),
                    Songs = a.Songs.
                                    Select(s => new
                                    {
                                        s.Name,
                                        Price = s.Price.ToString("f2"),
                                        WriterName = s.Writer.Name,
                                    })
                                    .OrderByDescending(s => s.Name)
                                    .ThenBy(s => s.WriterName)
                                    .ToArray()
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();

            foreach (var album in albums)
            {
                sb
                    .AppendLine($"-AlbumName: {album.AlbumName}")
                    .AppendLine($"-ReleaseDate: {album.ReleaseDate}")
                    .AppendLine($"-ProducerName: {album.ProducerName}")
                    .AppendLine("-Songs:");

                int songNumber = 1;

                foreach (var song in album.Songs)
                {
                    sb
                        .AppendLine($"---#{songNumber}")
                        .AppendLine($"---SongName: {song.Name}")
                        .AppendLine($"---Price: {song.Price}")
                        .AppendLine($"---Writer: {song.WriterName}");

                    songNumber++;
                }

                sb.AppendLine($"-AlbumPrice: {album.Toral_Album_Price}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            var songs = context.Songs
                .AsEnumerable()
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s => new
                {
                    SongName = s.Name,
                    WriterName = s.Writer.Name,
                    AlbumProdeucer = s.Album!.Producer!.Name,
                    Duration = s.Duration.ToString("c"),
                    Performer = s.SongPerformers
                                     .Select(p => new
                                     {
                                         Performer_Full_Name = $"{p.Performer.FirstName} {p.Performer.LastName}"
                                     })
                                     .OrderBy(p => p.Performer_Full_Name)
                })
                .OrderBy(s => s.SongName)
                .ThenBy(s => s.WriterName)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            int songNumber = 1;

            foreach (var song in songs)
            {
                sb
                    .AppendLine($"-Song #{songNumber++}")
                    .AppendLine($"---SongName: {song.SongName}")
                    .AppendLine($"---Writer: {song.WriterName}");

                foreach (var perf in song.Performer)
                {
                    sb.AppendLine($"---Performer: {perf.Performer_Full_Name}");
                }

                sb.AppendLine($"---AlbumProducer: {song.AlbumProdeucer}")
                    .AppendLine($"---Duration: {song.Duration}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
