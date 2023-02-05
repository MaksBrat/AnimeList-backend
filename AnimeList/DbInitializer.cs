using AnimeList.DAL;
using AnimeList.Domain.Entity.Animes;
using AnimeList.Domain.Entity.Genres;

namespace AnimeList
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (context.Animes.Any())
            {
                return;
            }

            var allAnime = new Anime[]
            {
                new Anime
                {
                    Title = "White Album 2",
                    Rating = 9,
                    Episodes = 12,
                    EpisodeDuration = 24,
                    AnimeType = Domain.Enum.AnimeType.Serial,
                    ReleaseDate = DateTime.Now,
                },
                new Anime
                {
                    Title = "Attack on titan",
                    Rating = 10,
                    Episodes = 22,
                    EpisodeDuration = 24,
                    AnimeType = Domain.Enum.AnimeType.Serial,
                    ReleaseDate = DateTime.Now,
                },
                new Anime
                {
                    Title = "Death Note",
                    Rating = 10,
                    Episodes = 48,
                    EpisodeDuration = 24,
                    AnimeType = Domain.Enum.AnimeType.Serial,
                    ReleaseDate = DateTime.Now,
                },
                new Anime
                {
                    Title = "Spy x Family",
                    Rating = 8,
                    Episodes = 12,
                    EpisodeDuration = 23,
                    AnimeType = Domain.Enum.AnimeType.Serial,
                    ReleaseDate = DateTime.Now,
                },
                new Anime
                {
                    Title = "Fate",
                    Rating = 9,
                    Episodes = 64,
                    EpisodeDuration = 24,
                    AnimeType = Domain.Enum.AnimeType.Serial,
                    ReleaseDate = DateTime.Now,
                },
                new Anime
                {
                    Title = "Golden Time",
                    Rating = 7,
                    Episodes = 12,
                    EpisodeDuration = 24,
                    AnimeType = Domain.Enum.AnimeType.Serial,
                    ReleaseDate = DateTime.Now,
                }
            };

            foreach (Anime anime in allAnime)
            {
                context.Animes.Add(anime);
            }

            context.SaveChanges();

            var genres = new Genre[]
            {
                new Genre{ Name = "Romance" },
                new Genre{ Name = "Action" },
                new Genre{ Name = "Drama" },
                new Genre{ Name = "Military" },
                new Genre{ Name = "Magic" },
                new Genre{ Name = "Comedy" },
                new Genre{ Name = "History" },
                new Genre{ Name = "Psychological" }
            };

            foreach (Genre genre in genres)
            {
                context.Genres.Add(genre);
            }

            context.SaveChanges();

            var animeGenres = new AnimeGenre[]
            {
                new AnimeGenre
                {
                    AnimeId = allAnime.Single(x => x.Title == "White Album 2").Id,
                    GenreId = genres.Single(x => x.Name == "Romance").Id
                },
                new AnimeGenre
                {
                    AnimeId = allAnime.Single(x => x.Title == "White Album 2").Id,
                    GenreId = genres.Single(x => x.Name == "Drama").Id
                },
                new AnimeGenre
                {
                    AnimeId = allAnime.Single(x => x.Title == "Attack on titan").Id,
                    GenreId = genres.Single(x => x.Name == "Drama").Id
                },
                new AnimeGenre
                {
                    AnimeId = allAnime.Single(x => x.Title == "Death Note").Id,
                    GenreId = genres.Single(x => x.Name == "Drama").Id
                },
                new AnimeGenre
                {
                    AnimeId = allAnime.Single(x => x.Title == "Death Note").Id,
                    GenreId = genres.Single(x => x.Name == "Psychological").Id
                },
                new AnimeGenre
                {
                    AnimeId = allAnime.Single(x => x.Title == "Death Note").Id,
                    GenreId = genres.Single(x => x.Name == "Action").Id
                },
                new AnimeGenre
                {
                    AnimeId = allAnime.Single(x => x.Title == "Spy x Family").Id,
                    GenreId = genres.Single(x => x.Name == "Action").Id
                },
                new AnimeGenre
                {
                    AnimeId = allAnime.Single(x => x.Title == "Spy x Family").Id,
                    GenreId = genres.Single(x => x.Name == "Comedy").Id
                },
                new AnimeGenre
                {
                    AnimeId = allAnime.Single(x => x.Title == "Fate").Id,
                    GenreId = genres.Single(x => x.Name == "Action").Id
                },
                new AnimeGenre
                {
                    AnimeId = allAnime.Single(x => x.Title == "Fate").Id,
                    GenreId = genres.Single(x => x.Name == "Drama").Id
                },
                new AnimeGenre
                {
                    AnimeId = allAnime.Single(x => x.Title == "Fate").Id,
                    GenreId = genres.Single(x => x.Name == "Magic").Id
                },
                new AnimeGenre
                {
                    AnimeId = allAnime.Single(x => x.Title == "Golden Time").Id,
                    GenreId = genres.Single(x => x.Name == "Romance").Id
                },
                new AnimeGenre
                {
                    AnimeId = allAnime.Single(x => x.Title == "Golden Time").Id,
                    GenreId = genres.Single(x => x.Name == "Drama").Id
                }
            };

            foreach (AnimeGenre animeGenre in animeGenres)
            {
                var animeGenreInDataBase = context.AnimeGenres.Where(
                    s =>
                            s.Anime.Id == animeGenre.AnimeId &&
                            s.Genre.Id == animeGenre.GenreId).SingleOrDefault();

                if (animeGenreInDataBase == null)
                {
                    context.AnimeGenres.Add(animeGenre);
                }
            }

            context.SaveChanges();
        }
    }
}
