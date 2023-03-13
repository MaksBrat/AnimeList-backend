using AnimeList.Common.Utility;

namespace AnimeList
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureCustomExceptionMiddleware(this WebApplication app)
        {
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();
        }
    }
}
