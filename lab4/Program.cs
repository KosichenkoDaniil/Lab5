using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using lab4;
using lab4.Middleware;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace lab4
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            string connection = builder.Configuration.GetConnectionString("MSSQL");

            // ��������� ����������� ��� ������� � �� � �������������� EF
            builder.Services.AddDbContext<BankDepositsContext>(options => options.UseSqlServer(connection));

                        builder.Services.AddIdentity<IdentityUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<BankDepositsContext>().
                    AddDefaultUI()
                    .AddDefaultTokenProviders(); ;
            builder.Services.AddControllersWithViews(options => {
                options.CacheProfiles.Add("ModelCache",
                    new CacheProfile()
                    {
                        Location = ResponseCacheLocation.Any,
                        Duration = 2 * 17 + 240
                    });
            });

            builder.Services.AddSession();
            builder.Services.AddRazorPages();
            //������������� MVC - ���������
            //services.AddControllersWithViews();
            var app = builder.Build();
            // ��������� ��������� ����������� ������
            app.UseStaticFiles();

            // ��������� ��������� ������
            app.UseSession();
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseDbInitializer();
            app.UseRouting();
            app.MapRazorPages();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();

            }); ;


            app.Run();
        }
    }
}
