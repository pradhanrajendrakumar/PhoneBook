using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PhoneBookApi;
using PhoneBookApi.HealthChecks;
using PhoneBookApi.Repositories;
using PhoneBookApi.Seed;
using PhoneBookApi.Services;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;
using Polly;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddW3CLogging((opt) =>
//{
//    opt.LoggingFields = Microsoft.AspNetCore.HttpLogging.W3CLoggingFields.All;
//});

var mongoClient = new MongoClient(builder.Configuration.GetConnectionString("default"));
var policy = Policy.Handle<MongoConnectionException>()
        .WaitAndRetry(10, i => TimeSpan.FromSeconds(30), (exception, timespan, retryCount, context) =>
        {
            // Log the retry information
            Console.WriteLine($"MongoDB connection failed. Retrying in {timespan.Seconds} seconds...");
        });

builder.Services.AddSingleton<IMongoClient>(sp => policy.Execute(() => mongoClient));
builder.Services.AddSingleton<IMongoDatabase>(x => x.GetRequiredService<IMongoClient>().GetDatabase("phonebook"));

builder.Services.AddSingleton<SeedData>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSingleton<MongoDbHealthCheck>();


builder.ConfigureHealthChecks();
builder.ConfigureLogger();



builder.Services.AddApiVersioning(opt =>
{
    opt.AssumeDefaultVersionWhenUnspecified = true;
    opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    opt.ReportApiVersions = true;

});

builder.Services.AddVersionedApiExplorer(opt =>
{
    opt.GroupNameFormat = "'v'VVV";
    opt.SubstituteApiVersionInUrl = true;
});

// Add services to the container.
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

//app.UseW3CLogging();
var seedData = app.Services.GetRequiredService<SeedData>();
seedData.EnsureSeedData();

app.UseSwagger();
app.UseSerilogRequestLogging();

app.UseSwaggerUI(options =>
{
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    foreach (var description in provider.ApiVersionDescriptions)
    {
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
    }
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    },
    ResponseWriter = WriteResponse
});

app.Run();

static Task WriteResponse(HttpContext context, HealthReport healthReport)
{
    context.Response.ContentType = "application/json; charset=utf-8";

    var options = new JsonWriterOptions { Indented = true };

    using var memoryStream = new MemoryStream();
    using (var jsonWriter = new Utf8JsonWriter(memoryStream, options))
    {
        jsonWriter.WriteStartObject();
        jsonWriter.WriteString("status", healthReport.Status.ToString());
        jsonWriter.WriteStartObject("results");

        foreach (var healthReportEntry in healthReport.Entries)
        {
            jsonWriter.WriteStartObject(healthReportEntry.Key);
            jsonWriter.WriteString("status",
                healthReportEntry.Value.Status.ToString());
            jsonWriter.WriteString("description",
                healthReportEntry.Value.Description);
            jsonWriter.WriteStartObject("data");

            foreach (var item in healthReportEntry.Value.Data)
            {
                jsonWriter.WritePropertyName(item.Key);

                JsonSerializer.Serialize(jsonWriter, item.Value,
                    item.Value?.GetType() ?? typeof(object));
            }

            jsonWriter.WriteEndObject();
            jsonWriter.WriteEndObject();
        }

        jsonWriter.WriteEndObject();
        jsonWriter.WriteEndObject();
    }

    return context.Response.WriteAsync(
        Encoding.UTF8.GetString(memoryStream.ToArray()));
}
