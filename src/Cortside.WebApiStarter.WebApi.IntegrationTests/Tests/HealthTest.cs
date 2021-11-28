using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Cortside.Health.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Cortside.WebApiStarter.WebApi.IntegrationTests.Tests {
    public class HealthTest : IClassFixture<TestWebApplicationFactory<Startup>> {
        private readonly TestWebApplicationFactory<Startup> fixture;
        private readonly ITestOutputHelper testOutputHelper;
        private readonly HttpClient testServerClient;

        public HealthTest(TestWebApplicationFactory<Startup> fixture, ITestOutputHelper testOutputHelper) {
            this.fixture = fixture;
            this.testOutputHelper = testOutputHelper;
            testServerClient = fixture.CreateClient(new WebApplicationFactoryClientOptions {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Test() {
            //arrange

            //act
            var success = false;
            var iterations = 0;
            HttpResponseMessage response = null;
            while (!success && iterations < 15) {
                response = await testServerClient.GetAsync("api/health").ConfigureAwait(false);
                success = response.IsSuccessStatusCode;
            }

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var s = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var respObj = JsonConvert.DeserializeObject<HealthModel>(s);
            Assert.True(respObj.Healthy);
        }
    }
}
