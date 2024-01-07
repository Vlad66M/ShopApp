using Blazored.Toast;
using Microsoft.EntityFrameworkCore;
using ShopApp.Components;

namespace ShopApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddBlazoredToast();

            builder.Services.AddDbContext<AppDBContext>(options =>
            options.UseSqlite("Data Source=store.db"));

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var dbContext = services.GetRequiredService<AppDBContext>();
                await dbContext.Database.MigrateAsync();
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            await app.RunAsync();
        }
        /*public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddBlazoredToast();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            Catalog.CreateDB();
            *//*Catalog.AddItem("Куртка зимняя", "Размер: 50", 10000, "https://www.blacksides.ru/upload/resize_cache/iblock/b79/900_1200_1/b796be624d46d4947e73bc53772dbda8.jpg");
            Catalog.AddItem("Ботинки зимние", "Размер: 42", 6000, "https://kupiteoptom.ru/d/img_7273_5.jpg");
            Catalog.AddItem("Шапка зимняя", "Материал: шерсть", 1000, "https://img.joomcdn.net/1bc1953d64310f5d53fbae7cee4062591af7f52d_original.jpeg");*//*

            app.Run();
        }*/
    }
}
