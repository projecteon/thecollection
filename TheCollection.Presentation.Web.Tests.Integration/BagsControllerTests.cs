namespace TheCollection.Presentation.Web.Tests.Integration {
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using TheCollection.Api.Controllers.Tea;
    using TheCollection.Presentation.Web;
    using Xunit;

    /// <summary>
    /// These tests have a very long startup as it initializes all startup logic, including imports from Exchange
    /// Consider solving this differently
    /// </summary>

    // https://docs.microsoft.com/en-us/aspnet/core/testing/integration-testing
    // https://github.com/aspnet/Mvc/issues/3410
    // https://github.com/aspnet/Razor/issues/1212#issuecomment-297885722
    [Trait(nameof(BagsController), "Integration tests")]
    public class BagsControllerTests {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        public BagsControllerTests() {
            var dir = System.IO.Directory.GetCurrentDirectory();
            dir = dir.Replace(@"TheCollection.Presentation.Web.Tests.Integration\bin\Debug\netcoreapp2.0", @"TheCollection.Presentation.Web");
            //_server = new TestServer(new WebHostBuilder()
            //    .UseStartup<Startup>());
            _server = new TestServer(Microsoft.AspNetCore.WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .UseContentRoot(dir));

            _client = _server.CreateClient();
        }

        [Fact]
        public async Task RoomsControllerDependenciesAreResolvedSuccessFully() {
            //var response = await _client.GetAsync("/api/Rooms/");

            //response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task MeetingsControllerDependenciesAreResolvedSuccessFully() {
            //var response = await _client.GetAsync("/api/Meetings/");

            //response.EnsureSuccessStatusCode();

            //await response.Content.ReadAsStringAsync();
        }
    }
}
