using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Driver;

namespace PhoneBookApi.HealthChecks
{
    public class MongoDbHealthCheck : IHealthCheck
    {
        private readonly IMongoClient _mongoClient;

        public MongoDbHealthCheck(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                await _mongoClient.ListDatabasesAsync(cancellationToken: cancellationToken);
                return HealthCheckResult.Healthy();
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("MongoDB connection failed", ex);
            }
        }
    }    

}
