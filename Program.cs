using Microsoft.AspNetCore.Authentication.Cookies;
using MvcLightphrm_Prject.Models.Calendar;
using MvcLightphrm_Prject.Models.Product;
using MvcLightphrm_Prject.Models.Site;
using MvcLightphrm_Prject.Models.User;

namespace MvcLightphrm_Prject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register your repositories
            builder.Services.AddScoped<ProductRepo>();
            builder.Services.AddScoped<SiteRepo>();
            builder.Services.AddScoped<UserRepo>();
            builder.Services.AddScoped<CalendarRepo>();




            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Admin/Adminlog";
                options.AccessDeniedPath = "/Admin/AccessDenied"; // You can create this view later
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
