using PhoneBookApi.HealthChecks;
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
                .AddCheck<MongoDbHealthCheck>("mongodb")
                .AddCheck<SeqHealthCheck>("Seq");
        }

        public static void ConfigureLogger(this WebApplicationBuilder builder)
        {
            var seqUrl = builder.Configuration.GetValue<string>("SeqUrl");
            Log.Logger = new LoggerConfiguration()
        .WriteTo.Seq(seqUrl, Serilog.Events.LogEventLevel.Verbose)
        .CreateLogger();
            builder.Host.UseSerilog();
            builder.Logging.AddSerilog();
        }
    }
}
