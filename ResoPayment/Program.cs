using System.Text.Json.Serialization;
using NLog.Web;
using ResoPayment.Extensions;
using ResoPayment.Middlewares;

var logger = NLog.LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config")).GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();
    // Add services to the container.

    builder.Services.AddControllers().AddJsonOptions(x =>
    {
	    x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    builder.Services.AddSwaggerGen();
    builder.Services.AddStackExchangeRedisCache(options =>
    {
	    options.Configuration = builder.Configuration["RedisConnectionString"];
    });
    builder.Services.AddDatabase();
    builder.Services.AddUnitOfWork();
    builder.Services.AddServices();
    builder.Services.AddJwtValidation();
    builder.Services.AddConfigSwagger();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    app.UseSwagger(options =>
    {
	    options.RouteTemplate = "pos-payment/{documentName}/swagger.json";
    });
	app.UseSwaggerUI(c =>
	{
        c.SwaggerEndpoint("v1/swagger.json","My Payment API");
        c.RoutePrefix = "pos-payment";
	});

        app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "Stop program because of exception");
}
finally
{
    NLog.LogManager.Shutdown();
}
