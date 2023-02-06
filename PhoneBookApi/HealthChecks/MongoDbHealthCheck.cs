using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace PhoneBookApi.HealthChecks
{
    public class MongoDbHealthCheck : IHealthCheck
    {
        private readonly IMongoDatabase _db;

        public MongoDbHealthCheck(IMongoDatabase mongoDatabase)
        {
            _db = mongoDatabase;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var healthCheckResultHealthy = await CheckMongoDBConnectionAsync();


            return healthCheckResultHealthy
                ? HealthCheckResult.Healthy("MongoDB health check success")
                : HealthCheckResult.Unhealthy("MongoDB health check failure");
        }

        private async Task<bool> CheckMongoDBConnectionAsync()
        {
            try
            {
                await _db.RunCommandAsync((Command<BsonDocument>)"{ping:1}");
            }

            catch(Exception)
            {
                return false;
            }

            return true;
        }
    }
}
