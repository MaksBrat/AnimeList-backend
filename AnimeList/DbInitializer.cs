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

            #region Anime

            var allAnime = new Anime[]
            {
                new Anime
                {
                    Title = "White Album 2",
                    Rating = 9,
                    Episodes = 12,
                    EpisodeDuration = 24,
                    AnimeType = Domain.Enums.AnimeType.Serial,
                    ReleaseDate = DateTime.Now,
                    PosterUrl = "https://www.nautiljon.com/images/anime/00/03/white_album_2_2630.webp"
                },
                new Anime
                {
                    Title = "Attack on titan",
                    Rating = 10,
                    Episodes = 22,
                    EpisodeDuration = 24,
                    AnimeType = Domain.Enums.AnimeType.Serial,
                    ReleaseDate = DateTime.Now,
                    PosterUrl = "https://desu.shikimori.one/uploads/poster/animes/16498/main-4c0035b92fad430c6721eb2e3779d384.webp"
                },
                new Anime
                {
                    Title = "Death Note",
                    Rating = 10,
                    Episodes = 48,
                    EpisodeDuration = 24,
                    AnimeType = Domain.Enums.AnimeType.Serial,
                    ReleaseDate = DateTime.Now,
                    PosterUrl = "https://m.media-amazon.com/images/M/MV5BNjRiNmNjMmMtN2U2Yi00ODgxLTk3OTMtMmI1MTI1NjYyZTEzXkEyXkFqcGdeQXVyNjAwNDUxODI@._V1_FMjpg_UX1000_.jpg"
                },
                new Anime
                {
                    Title = "Spy x Family",
                    Rating = 8,
                    Episodes = 12,
                    EpisodeDuration = 23,
                    AnimeType = Domain.Enums.AnimeType.Serial,
                    ReleaseDate = DateTime.Now,
                    PosterUrl = "https://resizing.flixster.com/oiRj9p9jfMCGBa3WA0zJ8TJ8Bwk=/ems.cHJkLWVtcy1hc3NldHMvdHZzZXJpZXMvMzBiMzU5YzktOGJjOC00ODVhLTk0Y2MtYmUwZjA1Yjk5YTgwLnBuZw=="
                },
                new Anime
                {
                    Title = "Fate/Zero",
                    Rating = 9,
                    Episodes = 64,
                    EpisodeDuration = 24,
                    AnimeType = Domain.Enums.AnimeType.Serial,
                    ReleaseDate = DateTime.Now,
                    PosterUrl = "https://static.wikia.nocookie.net/typemoon/images/f/fe/Fate_zero_anime_1st_season.jpg/revision/latest?cb=20211001233341"
                },
                new Anime
                {
                    Title = "Golden Time",
                    Rating = 7,
                    Episodes = 12,
                    EpisodeDuration = 24,
                    AnimeType = Domain.Enums.AnimeType.Serial,
                    ReleaseDate = DateTime.Now,
                    PosterUrl = "https://m.media-amazon.com/images/M/MV5BZTI0MDA5MWUtMWMyYS00NWM3LWE5ZmYtYTUxZmMxMGE5Y2IwXkEyXkFqcGdeQXVyNTIxNDgzOTg@._V1_FMjpg_UX1000_.jpg"
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
                    AnimeId = allAnime.Single(x => x.Title == "Fate/Zero").Id,
                    GenreId = genres.Single(x => x.Name == "Action").Id
                },
                new AnimeGenre
                {
                    AnimeId = allAnime.Single(x => x.Title == "Fate/Zero").Id,
                    GenreId = genres.Single(x => x.Name == "Drama").Id
                },
                new AnimeGenre
                {
                    AnimeId = allAnime.Single(x => x.Title == "Fate/Zero").Id,
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

            #endregion           
        }
    }
}
