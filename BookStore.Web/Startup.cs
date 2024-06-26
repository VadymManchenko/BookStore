using System.Data;
using System.Data.SqlClient;
using BookStore.Contractors;
using BookStore.Interfaces;
using BookStore.Memory;
using BookStore.Messages;
using BookStore.Services;

namespace BookStore.Web;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();
        services.AddDistributedMemoryCache();
        services.AddSession(options =>
            {
                options.IdleTimeout= TimeSpan.FromHours(12);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            }
        );
        services.AddSingleton<IBookRepository, BookRepository>()
            .AddSingleton<IOrderRepository, OrderRepository>()
            .AddSingleton<INotificationService, DebugNotificationService>()
            .AddSingleton<IDeliveryService, PostamateDeliveryService>()
            .AddSingleton<BookService>();

        /*services.AddSingleton<Func<IDbConnection>>(serviceProvider => serviceProvider.GetService<IDbConnection>);
        
        services.AddTransient<IDbConnection, SqlConnection>();

        services.AddScoped<IDbConnection, SqlConnection>();*/
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();

        app.UseSession();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                "default",
                "{controller=Home}/{action=Index}/{id?}");
        });
    }
}