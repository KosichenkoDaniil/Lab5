using Microsoft.AspNetCore.Builder;

namespace lab4.Middleware
{
    public static class DbInitializerExtensions
    {
        public static IApplicationBuilder UseDbInitializer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DbInitializerMiddleware>();
        }

    }
}