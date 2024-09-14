using System.Diagnostics.CodeAnalysis;
using Testcontainers.RabbitMq;

namespace Postech.GroupEight.ContactIntegration.IntegrationTests.Configurations.Factories
{
    [ExcludeFromCodeCoverage]
    public static class TestContainerFactory
    {
        private static RabbitMqContainer? _container;
        private static readonly Lazy<Task> _initializeTask = new(InitializeAsync);

        public static string ConnectionString { get; private set; } = string.Empty;

        private static async Task InitializeAsync()
        {
            if (_container is null)
            {
                _container = new RabbitMqBuilder()
                    .WithImage("rabbitmq:3.11")
                    .WithPortBinding(5672, 5672)
                    .WithUsername("guest")
                    .WithPassword("guest")
                    .Build();
                await _container.StartAsync();
                ConnectionString = _container.GetConnectionString();
            }
        }

        public static Task EnsureInitialized() => _initializeTask.Value;

        public static async Task DisposeAsync()
        {
            if (_container is not null)
            {
                await _container.StopAsync();
                await _container.DisposeAsync();
                _container = null;
            }
        }
    }
}
