using TallerIdwm.src.data;
using TallerIdwm.src.interfaces;
using TallerIdwm.src.models;
using TallerIdwm.src.repositories;

using Serilog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;



Log.Logger = new LoggerConfiguration()
    .CreateLogger();

try {
    Log.Information("Starting up the application...");
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddControllers();;

    
    builder.Services.AddDbContext<StoreContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<UnitOfWork>();


    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithThreadId()
            .Enrich.WithMachineName();
    });


    var app = builder.Build();
    DbInitializer.InitDb(app);
    app.MapControllers();
    app.Run();


} catch (Exception ex) {
    Log.Fatal(ex, "Application start-up failed");
} finally {
    Log.CloseAndFlush();
}

