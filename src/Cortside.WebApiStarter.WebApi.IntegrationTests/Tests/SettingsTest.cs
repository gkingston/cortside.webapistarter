using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Cortside.WebApiStarter.WebApi.IntegrationTests.Tests {
    public class SettingsTest : IClassFixture<TestWebApplicationFactory<Startup>> {
        private readonly TestWebApplicationFactory<Startup> fixture;
        private readonly ITestOutputHelper testOutputHelper;
        private readonly HttpClient testServerClient;

        public SettingsTest(TestWebApplicationFactory<Startup> fixture, ITestOutputHelper testOutputHelper) {
            this.testOutputHelper = testOutputHelper;
            testServerClient = fixture.CreateClient(new WebApplicationFactoryClientOptions {
                AllowAutoRedirect = false
            });
        }
    }
}
