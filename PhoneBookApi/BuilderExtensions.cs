using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;
using PhoneBookApi.HealthChecks;
using PhoneBookApi.Seed;
using Polly;
using Serilog;

namespace PhoneBookApi
{
    public static class BuilderExtensions
    {
       public static void ConfigureHealthChecks(this WebApplicationBuilder builder)
        {
            var seqUrl = builder.Configuration.GetValue<string>("SeqUrl");
            var seqHealthCheck = new SeqHealthCheck(seqUrl);
            builder.Services.AddSingleton(seqHealthCheck);
            builder.Services.AddHealthChecks()
                .AddCheck("self",() => HealthCheckResult.Healthy())
                .AddCheck<MongoDbHealthCheck>("mongodb_connection",tags: new[] {"ready"})
                .AddCheck<MongoDbSeedHealthCheck>("mongodb_dataSeed",tags: new[] { "ready" })
                .AddCheck<SeqHealthCheck>("Seqlogging_connection",tags: new[] { "ready" });
        }

        public static void ConfigureLogger(this WebApplicationBuilder builder)
        {
            var seqUrl = builder.Configuration["Serilog:SeqServerUrl"];
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("ApplicationContext",Program.AppName)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(seqUrl,Serilog.Events.LogEventLevel.Verbose)
                .CreateLogger();
            builder.Host.UseSerilog((builderContext,config) =>
            {
                config
                    .MinimumLevel.Information()
                    .Enrich.FromLogContext()
                    .WriteTo.Console();
            });
            builder.Logging.AddSerilog();
        }

        public static void ConfigureMongoDb(this WebApplicationBuilder builder)
        {
            var mongoClient = new MongoClient(builder.Configuration.GetSection(MongoSettingOptions.MongoSettings).GetValue<string>(MongoSettingOptions.Connection));
            var policy = Policy.Handle<MongoConnectionException>()
                    .WaitAndRetry(10,i => TimeSpan.FromSeconds(30),(exception,timespan,retryCount,context) =>
                    {
                        // Log the retry information
                        Console.WriteLine($"MongoDB connection failed. Retrying in {timespan.Seconds} seconds...");
                    });

            builder.Services.AddSingleton<IMongoClient>(sp => policy.Execute(() => mongoClient));
            builder.Services.AddSingleton<IMongoDatabase>(x => x.GetRequiredService<IMongoClient>().GetDatabase(builder.Configuration.GetSection(MongoSettingOptions.MongoSettings).GetValue<string>(MongoSettingOptions.DatabaseName)));
            builder.Services.AddSingleton<SeedData>();
        }
    }
   
}
