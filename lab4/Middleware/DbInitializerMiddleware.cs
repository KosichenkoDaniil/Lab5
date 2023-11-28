using lab4.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace lab4.Middleware
{
    public class DbInitializerMiddleware
    {
        private readonly RequestDelegate _next;
        public DbInitializerMiddleware(RequestDelegate next)
        {
            // инициализация базы данных 
            _next = next;
        }
        public Task Invoke(HttpContext context)
        {
            if (!(context.Session.Keys.Contains("starting")))
            {
                DbInitializer.Initialize(context.RequestServices.GetRequiredService<BankDepositsContext>());
                context.Session.SetString("starting", "Yes");
            }

            // Вызов следующего делегата / компонента middleware в конвейере
            return _next.Invoke(context);
        }
    }

}