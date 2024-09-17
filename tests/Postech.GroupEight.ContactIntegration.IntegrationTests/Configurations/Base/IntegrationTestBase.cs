using Microsoft.Extensions.DependencyInjection;
using Postech.GroupEight.ContactIntegration.IntegrationTests.Configurations.Factories;
using Postech.GroupEight.ContactIntegration.IntegrationTests.Fixtures;
using System.Diagnostics.CodeAnalysis;

namespace Postech.GroupEight.ContactIntegration.IntegrationTests.Configurations.Base
{
    [ExcludeFromCodeCoverage]
    public class IntegrationTestBase : IClassFixture<IntegrationTestFixture>
    {
        protected readonly HttpClient HttpClient;
        protected readonly ContactIntegrationApplicationFactory WebApplicationFactory;

        protected IntegrationTestBase(IntegrationTestFixture fixture)
        {
            WebApplicationFactory = fixture.WebApplicationFactory;
            HttpClient = WebApplicationFactory.CreateClient();
        }

        protected T GetService<T>()
            where T : notnull
        {
            IServiceScope scope = WebApplicationFactory.Services.CreateScope();
            return scope.ServiceProvider.GetRequiredService<T>();
        }
    }
}
