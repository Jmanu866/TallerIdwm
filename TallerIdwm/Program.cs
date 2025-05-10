using System.Security.Claims;
using System.Text;


using TallerIdwm.middleware;

using TallerIdwm.src.data;
using TallerIdwm.src.interfaces;
using TallerIdwm.src.models;
using TallerIdwm.src.repositories;
using TallerIdwm.src.services;

using Serilog;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ayudantia.Src.Data;



Log.Logger = new LoggerConfiguration()
    .CreateLogger();

try
{
    // creacion de patron del builder de .net para crear la aplicacion
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddControllers();
    builder.Services.AddTransient<ExceptionMIddleware>();
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<ITokenServices, TokenService>();
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
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!)),
            RoleClaimType = ClaimTypes.Role
        };
    });
    builder.Services.AddDbContext<StoreContext>(options =>
     options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithThreadId()
            .Enrich.WithMachineName());

    var app = builder.Build();
    app.UseMiddleware<ExceptionMIddleware>();
    await DbInitializer.InitDb(app);
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    // corremos la aplicacion
    // app.Run() es el metodo que se encarga de correr la aplicacion y escuchar las peticiones
    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}

