using Postech.GroupEight.ContactIntegration.IntegrationTests.Configurations.Factories;
using System.Diagnostics.CodeAnalysis;

namespace Postech.GroupEight.ContactIntegration.IntegrationTests.Fixtures
{
    [ExcludeFromCodeCoverage]
    public class IntegrationTestFixture : IAsyncLifetime
    {
        public ContactIntegrationApplicationFactory WebApplicationFactory { get; private set; }
        public static string ConnectionString => TestContainerFactory.ConnectionString;

        public async Task InitializeAsync()
        {
            await TestContainerFactory.EnsureInitialized();
            WebApplicationFactory = new(ConnectionString);
        }

        public async Task DisposeAsync()
        {
            await TestContainerFactory.DisposeAsync();

        }    
    }
}
