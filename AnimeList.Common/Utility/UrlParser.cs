using System.Text.RegularExpressions;

namespace AnimeList.Common.Utility
{
    public static class UrlParser
    {
        private static string _pattern = "v=([^&]+)";
        private const string YOUTUBE_URL = "https://www.youtube.com/embed/";
        private const string AUTOPLAY_QUERY = "?autoplay=1";

        public static string ParseTrailerUrl(string youtubeUrl)
        {
            string videoId = GetVideoId(youtubeUrl);
            return YOUTUBE_URL + videoId + AUTOPLAY_QUERY;
        }

        private static string GetVideoId(string youtubeUrl) =>
           Regex.Match(youtubeUrl, _pattern).Groups[1].Value;
    }
}
