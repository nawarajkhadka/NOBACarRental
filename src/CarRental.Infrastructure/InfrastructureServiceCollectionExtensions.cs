using System.Text;
using CarRental.Application.Auth;
using CarRental.Application.Bookings;
using CarRental.Application.Pricing;
using CarRental.Application.Repositories;
using CarRental.Domain.Pricing;
using CarRental.Infrastructure.Auth;
using CarRental.Infrastructure.Identity;
using CarRental.Infrastructure.Persistence;
using CarRental.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace CarRental.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        bool useInMemoryDatabase = false)
    {
        // useInMemoryDatabase lets WebApplicationFactory-based integration tests run without a real MSSQL instance.
        services.AddDbContext<AppDbContext>(options =>
        {
            if (useInMemoryDatabase)
            {
                options.UseInMemoryDatabase(configuration["InMemoryDatabaseName"] ?? "CarRentalTests");
            }
            else
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
        });

        services
            .AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
            ?? throw new InvalidOperationException("Jwt configuration section is missing.");

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SigningKey))
                };
            });

        services.AddScoped<ITokenService, TokenService>();

        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<ICarRepository, CarRepository>();
        services.AddScoped<ICarCategoryRepository, CarCategoryRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        services.AddScoped<IBookingService, BookingService>();

        services.AddScoped<IPriceCalculator, SmallCarPriceCalculator>();
        services.AddScoped<IPriceCalculator, CombiPriceCalculator>();
        services.AddScoped<IPriceCalculator, TruckPriceCalculator>();
        services.AddScoped<IPriceCalculatorFactory, PriceCalculatorFactory>();

        services.AddHostedService<RoleSeeder>();

        return services;
    }
}
