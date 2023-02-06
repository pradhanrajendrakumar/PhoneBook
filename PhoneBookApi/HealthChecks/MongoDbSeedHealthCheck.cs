using Microsoft.Extensions.Diagnostics.HealthChecks;
using PhoneBookApi.Seed;

namespace PhoneBookApi.HealthChecks
{
    public class MongoDbSeedHealthCheck : IHealthCheck
    {
        private readonly SeedData _seedData;

        public MongoDbSeedHealthCheck(SeedData seedData)
        {
            _seedData = seedData;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,CancellationToken cancellationToken = default)
        {
            try
            {
                await Task.FromResult(_seedData.EnsureSeedData);
                return HealthCheckResult.Healthy("Data seed for phone API sucessful!");
            }
            catch(Exception ex)
            {
                return HealthCheckResult.Unhealthy("Data seed failed with mongo db.",ex);
            }
        }
    }
}
