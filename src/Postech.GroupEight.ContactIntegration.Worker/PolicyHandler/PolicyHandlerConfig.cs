using Polly;
using Polly.CircuitBreaker;

namespace Postech.GroupEight.ContactIntegration.Worker.PolicyHandler
{
    public static class PolicyHandlerConfig
    {
        public static AsyncCircuitBreakerPolicy GetCircuitBreakerPolicy(ILogger logger)
        {
            return Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: 3,
                    durationOfBreak: TimeSpan.FromSeconds(30),
                    onBreak: (exception, timespan) =>
                    {
                        logger.LogError($"Circuit broken due to: {exception.Message}. Breaking for {timespan.TotalSeconds} seconds.");
                    },
                    onReset: () =>
                    {
                        logger.LogInformation("Circuit reset.");
                    },
                    onHalfOpen: () =>
                    {
                        logger.LogInformation("Circuit is half-open. Next call is a trial.");
                    });

        }
    }
}
