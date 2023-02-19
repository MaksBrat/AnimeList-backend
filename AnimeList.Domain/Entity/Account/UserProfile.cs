
namespace AnimeList.Domain.Entity.Account
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Age { get; set; }
        public int FileModelId { get; set; }
        public FileModel FileModel { get; set; }
        public DateTime RegistratedAt { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<UserAnimeList> AnimeList { get; set; }
    }
}
