namespace TheCollection.Presentation.Web.Tests.Integration {
    using System;
    using System.IO;
    using FakeItEasy;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using NodaTime;
    using NodaTime.Testing;
    using TheCollection.Api.Controllers;
    using TheCollection.Api.Controllers.Tea;
    using TheCollection.Application.Services.Commands.Tea;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;
    using TheCollection.Presentation.Web;
    using Xunit;
    using Xunit.Abstractions;

    [Trait(nameof(ServiceCollection), "Integration tests")]
    public class ServiceCollectionTests : IClassFixture<StartUpFixture> {
        ServiceCollection ServiceCollection { get; }

        public ServiceCollectionTests(StartUpFixture fixture, ITestOutputHelper testOutputHelper) {
            ServiceCollection = fixture.ServiceCollection;
            ServiceCollection.AddSingleton(testOutputHelper);
            ServiceCollection.AddSingleton<IClock>(FakeClock.FromUtc(2018, 1, 1));
        }

        #region Controllers

        [Fact]
        public void AbleToResolveBagsController() {
            //  Mimic internal asp.net core logic.
            ServiceCollection.AddTransient<BagsController>();
            var serviceProvider = ServiceCollection.BuildServiceProvider();

            var command = serviceProvider.GetService<BagsController>();

            Assert.NotNull(command);
        }

        [Fact]
        public void AbleToResolveBagTypesController() {
            //  Mimic internal asp.net core logic.
            ServiceCollection.AddTransient<BagTypesController>();
            var serviceProvider = ServiceCollection.BuildServiceProvider();

            var command = serviceProvider.GetService<BagTypesController>();

            Assert.NotNull(command);
        }

        [Fact]
        public void AbleToResolveBrandsController() {
            //  Mimic internal asp.net core logic.
            ServiceCollection.AddTransient<BrandsController>();
            var serviceProvider = ServiceCollection.BuildServiceProvider();

            var command = serviceProvider.GetService<BrandsController>();

            Assert.NotNull(command);
        }

        [Fact]
        public void AbleToResolveCountriesController() {
            //  Mimic internal asp.net core logic.
            ServiceCollection.AddTransient<CountriesController>();
            var serviceProvider = ServiceCollection.BuildServiceProvider();

            var command = serviceProvider.GetService<CountriesController>();

            Assert.NotNull(command);
        }

        [Fact]
        public void AbleToResolveDashboardsController() {
            //  Mimic internal asp.net core logic.
            ServiceCollection.AddTransient<DashboardsController>();
            var serviceProvider = ServiceCollection.BuildServiceProvider();

            var command = serviceProvider.GetService<DashboardsController>();

            Assert.NotNull(command);
        }

        [Fact]
        public void AbleToResolveFileUploadsController() {
            //  Mimic internal asp.net core logic.
            ServiceCollection.AddTransient<FileUploadsController>();
            var serviceProvider = ServiceCollection.BuildServiceProvider();

            var command = serviceProvider.GetService<FileUploadsController>();

            Assert.NotNull(command);
        }

        #endregion (Controllers)

        [Fact]
        public void AbleToResolveCreateBagsCountByBagTypesCommandHandler() {
            var serviceProvider = ServiceCollection.BuildServiceProvider();

            var command = serviceProvider.GetService<IAsyncCommandHandler<CreateBagsCountByBagTypesCommand>>();

            Assert.NotNull(command);
        }

        [Fact]
        public void AbleToResolveSchedulerHostedService() {
            var serviceProvider = ServiceCollection.BuildServiceProvider();

            var hostedService = serviceProvider.GetService<IHostedService>();

            Assert.NotNull(hostedService);
        }
    }

    public class StartUpFixture : IDisposable {
        public StartUpFixture() {
            var configuration = new ConfigurationBuilder()
                                        .SetBasePath(Directory.GetCurrentDirectory())
                                        .AddJsonFile("appsettings.json")
                                        .Build();
            ServiceCollection = new ServiceCollection();
            ServiceCollection.AddSingleton(typeof(Microsoft.Extensions.Logging.ILogger<>), typeof(XunitLogger<>));

            var fakeEnvironment = A.Fake<Microsoft.AspNetCore.Hosting.IHostingEnvironment>();
            A.CallTo(() => fakeEnvironment.EnvironmentName).Returns(Microsoft.AspNetCore.Hosting.EnvironmentName.Development);

            var target = new Startup(configuration, fakeEnvironment);
            target.ConfigureServices(ServiceCollection);

            // Override configuration in startup untill I can find good fakes/mocks
            var fakeUrlHelper = A.Fake<IUrlHelper>();
            ServiceCollection.AddSingleton(fakeUrlHelper);

            var fakeImageRepository = A.Fake<IImageRepository>();
            ServiceCollection.AddSingleton<IImageRepository>(fakeImageRepository);            
        }

        public ServiceCollection ServiceCollection { get; }

        public void Dispose() {
        }
    }
}
