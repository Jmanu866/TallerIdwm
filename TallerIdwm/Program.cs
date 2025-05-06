using System.Security.Claims;
using System.Text;
using TallerIdwm.src.data;
using TallerIdwm.src.interfaces;
using TallerIdwm.src.models;
using TallerIdwm.src.repositories;
using TallerIdwm.src.services;

using Serilog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;



Log.Logger = new LoggerConfiguration()
    .CreateLogger();

try {
    Log.Information("Starting up the application...");
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddControllers();;

    
    
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<UnitOfWork>();
    builder.Services.AddIdentity<User, IdentityRole>(opt =>
    {
        opt.User.RequireUniqueEmail = true;
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequiredLength = 6;
        opt.SignIn.RequireConfirmedEmail = false;
    })
    .AddEntityFrameworkStores<StoreContext>();
    builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })

    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SignInKey"]!)),
            RoleClaimType = ClaimTypes.Role
        };
    });
    builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
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
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();


} catch (Exception ex) {
    Log.Fatal(ex, "Application start-up failed");
} finally {
    Log.CloseAndFlush();
}

