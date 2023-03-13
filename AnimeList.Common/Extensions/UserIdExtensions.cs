namespace AnimeList.Common.Extentions
{
    public static class UserIdExtensions
    {
        private static int id = 0;
        public static int GetId => ++id; 
    }
}
