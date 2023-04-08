using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ResoPayment.ApplicationCore.Implementations;
using ResoPayment.ApplicationCore.Interfaces;
using ResoPayment.Constants;
using ResoPayment.Infrastructure.Models;
using ResoPayment.Service.Implements;
using ResoPayment.Service.Interfaces;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ResoPayment.Extensions;

public static class DependencyService
{
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables(EnvironmentVariableConstant.Prefix).Build();
        services.AddDbContext<PosPaymentContext>(options =>
            options.UseSqlServer(CreateConnectionString(configuration)));
        return services;
    }

    private static string CreateConnectionString(IConfiguration configuration)
    {
        string connectionString =
            $"Server={configuration.GetValue<string>(DatabaseConstant.Host)},{configuration.GetValue<string>(DatabaseConstant.Port)};User Id={configuration.GetValue<string>(DatabaseConstant.UserName)};Password={configuration.GetValue<string>(DatabaseConstant.Password)};Database={configuration.GetValue<string>(DatabaseConstant.Database)}";
        return connectionString;
    }

    public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork<PosPaymentContext>, UnitOfWork<PosPaymentContext>>();
        return services;
    }

    public static IServiceCollection AddJwtValidation(this IServiceCollection services)
    {
        IConfiguration configuration = new ConfigurationBuilder().AddEnvironmentVariables(EnvironmentVariableConstant.Prefix).Build();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidIssuer = configuration.GetValue<string>(JwtConstant.Issuer),
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(configuration.GetValue<string>(JwtConstant.SecretKey)))
            };
        });
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IVnPayServices, VnPayService>();
        return services;
    }

    public static IServiceCollection AddConfigSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo() { Title = "Pos System", Version = "v1" });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
            options.MapType<TimeOnly>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "time",
                Example = OpenApiAnyFactory.CreateFromJson("\"13:45:42.0000000\"")
            });
        });
        return services;
    }
}