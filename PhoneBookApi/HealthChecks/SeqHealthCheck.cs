using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace PhoneBookApi.HealthChecks
{
    public class SeqHealthCheck : IHealthCheck
    {
        private readonly string _seqUrl;

        public SeqHealthCheck(string seqUrl)
        {
            _seqUrl = seqUrl;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var response = await client.GetAsync($"{_seqUrl}", cancellationToken);
                    if (response.IsSuccessStatusCode)
                    {
                        return HealthCheckResult.Healthy();
                    }
                    else
                    {
                        return HealthCheckResult.Unhealthy("Seq ping API returned non-success status code");
                    }
                }
                catch (Exception ex)
                {
                    return HealthCheckResult.Unhealthy("Seq ping API call failed", ex);
                }
            }
        }
    }
}