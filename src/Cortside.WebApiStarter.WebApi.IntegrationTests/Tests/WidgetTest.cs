using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Cortside.WebApiStarter.WebApi.Models.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Cortside.WebApiStarter.WebApi.IntegrationTests.Tests {

    public class WidgetTest : IClassFixture<IntegrationTestFactory<Startup>> {
        private readonly IntegrationTestFactory<Startup> fixture;
        private readonly ITestOutputHelper testOutputHelper;
        private readonly HttpClient testServerClient;

        public WidgetTest(IntegrationTestFactory<Startup> fixture, ITestOutputHelper testOutputHelper) {
            this.fixture = fixture;
            this.testOutputHelper = testOutputHelper;
            testServerClient = fixture.CreateClient(new WebApplicationFactoryClientOptions {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task ShouldCreateWidget() {
            //arrange
            var request = new WidgetRequest() {
                Text = Guid.NewGuid().ToString(),
                Width = 100,
                Height = 100
            };

            var requestBody = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            //act
            var response = await testServerClient.PostAsync("/api/v1/widgets", requestBody).ConfigureAwait(false);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task ShouldGetWidget() {
            //arrange
            var id = fixture.Db.Widgets.First().WidgetId;

            //act
            var response = await testServerClient.GetAsync($"api/v1/widgets/{id}").ConfigureAwait(false);

            //assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
