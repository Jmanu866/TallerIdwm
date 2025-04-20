using Serilog;


Log.Logger = new LoggerConfiguration()
    .CreateLogger();

try {
    Log.Information("Starting up the application...");
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddControllers();
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console()
    );


    var app = builder.Build();
    app.MapControllers();
    app.Run();


} catch (Exception ex) {
    Log.Fatal(ex, "Application start-up failed");
} finally {
    Log.CloseAndFlush();
}

