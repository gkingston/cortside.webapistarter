using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Cortside.Health.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Acme.WebApiStarter.WebApi.IntegrationTests.Tests {
    public class HealthTest : IClassFixture<IntegrationTestFactory<Startup>> {
        private readonly IntegrationTestFactory<Startup> fixture;
        private readonly ITestOutputHelper testOutputHelper;
        private readonly HttpClient testServerClient;

        public HealthTest(IntegrationTestFactory<Startup> fixture, ITestOutputHelper testOutputHelper) {
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
            var respObj = JsonConvert.DeserializeObject<HealthModel>(response.Content.ReadAsStringAsync().Result);
            Assert.True(respObj.Healthy);
        }
    }
}
