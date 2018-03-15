namespace TheCollection.Web.Extensions {
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.Documents;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using NodaTime;
    using TheCollection.Api;
    using TheCollection.Application.Services;
    using TheCollection.Application.Services.Commands;
    using TheCollection.Application.Services.Commands.Tea;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Application.Services.Queries;
    using TheCollection.Application.Services.Queries.Tea;
    using TheCollection.Application.Services.Translators;
    using TheCollection.Application.Services.Translators.Tea;
    using TheCollection.Data.DocumentDB.Repositories;
    using TheCollection.Domain;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;
    using TheCollection.Domain.Tea;
    using TheCollection.Infrastructure.Logging;
    using TheCollection.Infrastructure.Scheduling;
    using TheCollection.Infrastructure.Scheduling.Tea;
    using TheCollection.Web.Constants;
    using TheCollection.Web.Repositories;

    public static class IServiceCollectionExtensions {
        public static IServiceCollection WireDependencies(this IServiceCollection services) {
            services.AddSingleton<IClock>(SystemClock.Instance);
            services.AddSingleton(typeof(ILogger<>), typeof(ConsoleLogger<>));

            services.AddTranslators();
            services.AddRepositories();
            services.AddQueries();
            services.AddCommands();
            services.AddNotificationServices();
            services.AddSchedulingServices();
            return services;
        }

        public static IServiceCollection AddTranslators(this IServiceCollection services) {
            #region Domain models to view models
            services.AddSingleton<IAsyncTranslator<Domain.RefValue, Application.Services.ViewModels.RefValue>, RefValueToRefValueTranslator>();
            services.AddSingleton<IAsyncTranslator<Domain.Tea.Bag, Application.Services.ViewModels.Tea.Bag>, BagToBagViewModelTranslator>();
            services.AddSingleton<IAsyncTranslator<Domain.Tea.BagType, Application.Services.ViewModels.Tea.BagType>, BagTypeToBagTypeTranslator>();
            services.AddSingleton<IAsyncTranslator<Domain.Tea.Brand, Application.Services.ViewModels.Tea.Brand>, BrandToBrandTranslator>();
            services.AddSingleton<IAsyncTranslator<Domain.Tea.Country, Application.Services.ViewModels.Tea.Country>, CountryToCountryViewModelTranslator>();
            #endregion (Domain models to view models)

            #region View models to domain models
            services.AddSingleton<ITranslator<Application.Services.ViewModels.RefValue, Domain.RefValue>, RefValueDtoToRefValueTranslator>();
            services.AddSingleton<ITranslator<Application.Services.ViewModels.Tea.Bag, Domain.Tea.Bag, Domain.Tea.Bag>, BagViewModelToUpdateBagTranslator>();
            services.AddSingleton<IAsyncTranslator<Application.Services.ViewModels.Tea.Bag, Domain.Tea.Bag>, BagViewModelToCreateBagTranslator>();
            services.AddSingleton<ITranslator<Application.Services.ViewModels.Tea.BagType, Domain.Tea.BagType>, BagTypeDtoToBagTypeTranslator>();
            services.AddSingleton<ITranslator<Application.Services.ViewModels.Tea.Brand, Domain.Tea.Brand>, BrandDtoToBrandTranslator>();
            services.AddSingleton<ITranslator<Application.Services.ViewModels.Tea.Country, Domain.Tea.Country>, CountryViewModelToCountryTranslator>();
            #endregion (View models to domain models)

            services.AddSingleton<ITranslator<ICommandResult, IActionResult>, ICommandResultToIActionResultTranslator>();
            services.AddSingleton<ITranslator<IQueryResult, IActionResult>, IQueryResultToIActionResultTranslator>();
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services) {
            services.AddScoped<IGetRepository<IApplicationUser>, WebUserRepository>();
            services.AddSingleton<ILinqSearchRepository<IApplicationUser>, WebUserRepository>();

            services.AddScoped<IActivityAuthorizer, ActivityAuthorizer>();
            services.AddSingleton<ILinqSearchRepository<IActivity>>(serviceProvider => {
                var documentClient = serviceProvider.GetService<IDocumentClient>();
                return new SearchRepository<IActivity>(documentClient, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.AspNetIdentity);
            });

            #region Bag
            services.AddSingleton<ILinqSearchRepository<Bag>>(serviceProvider => {
                var documentClient = serviceProvider.GetService<IDocumentClient>();
                return new SearchRepository<Bag>(documentClient, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.Bags);
            });
            services.AddSingleton<ISearchRepository<Bag>>(serviceProvider => {
                var documentClient = serviceProvider.GetService<IDocumentClient>();
                return new SearchRepository<Bag>(documentClient, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.Bags);
            });
            services.AddSingleton<IGetRepository<Bag>>(serviceProvider => {
                var documentClient = serviceProvider.GetService<IDocumentClient>();
                return new GetRepository<Bag>(documentClient, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.Bags);
            });
            services.AddSingleton<ICreateRepository<Bag>>(serviceProvider => {
                var documentClient = serviceProvider.GetService<IDocumentClient>();
                return new CreateRepository<Bag>(documentClient, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.Bags);
            });
            services.AddSingleton<IUpdateRepository<Bag>>(serviceProvider => {
                var documentClient = serviceProvider.GetService<IDocumentClient>();
                return new UpdateRepository<Bag>(documentClient, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.Bags);
            });
            #endregion (Bag)

            #region BagType
            services.AddSingleton<ISearchRepository<BagType>>(serviceProvider => {
                var documentClient = serviceProvider.GetService<IDocumentClient>();
                return new SearchRepository<BagType>(documentClient, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.BagTypes);
            });
            services.AddSingleton<IGetRepository<BagType>>(serviceProvider => {
                var documentClient = serviceProvider.GetService<IDocumentClient>();
                return new GetRepository<BagType>(documentClient, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.BagTypes);
            });
            services.AddSingleton<ICreateRepository<BagType>>(serviceProvider => {
                var documentClient = serviceProvider.GetService<IDocumentClient>();
                return new CreateRepository<BagType>(documentClient, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.BagTypes);
            });
            services.AddSingleton<IUpdateRepository<BagType>>(serviceProvider => {
                var documentClient = serviceProvider.GetService<IDocumentClient>();
                return new UpdateRepository<BagType>(documentClient, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.BagTypes);
            });
            #endregion (BagType)

            #region Brand
            services.AddSingleton<ISearchRepository<Brand>>(serviceProvider => {
                var documentClient = serviceProvider.GetService<IDocumentClient>();
                return new SearchRepository<Brand>(documentClient, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.Brands);
            });
            services.AddSingleton<IGetRepository<Brand>>(serviceProvider => {
                var documentClient = serviceProvider.GetService<IDocumentClient>();
                return new GetRepository<Brand>(documentClient, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.Brands);
            });
            services.AddSingleton<ICreateRepository<Brand>>(serviceProvider => {
                var documentClient = serviceProvider.GetService<IDocumentClient>();
                return new CreateRepository<Brand>(documentClient, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.Brands);
            });
            services.AddSingleton<IUpdateRepository<Brand>>(serviceProvider => {
                var documentClient = serviceProvider.GetService<IDocumentClient>();
                return new UpdateRepository<Brand>(documentClient, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.Brands);
            });
            #endregion (Brand)

            #region Country
            services.AddSingleton<ISearchRepository<Country>>(serviceProvider => {
                var documentClient = serviceProvider.GetService<IDocumentClient>();
                return new SearchRepository<Country>(documentClient, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.Countries);
            });
            services.AddSingleton<IGetRepository<Country>>(serviceProvider => {
                var documentClient = serviceProvider.GetService<IDocumentClient>();
                return new GetRepository<Country>(documentClient, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.Countries);
            });
            services.AddSingleton<ICreateRepository<Country>>(serviceProvider => {
                var documentClient = serviceProvider.GetService<IDocumentClient>();
                return new CreateRepository<Country>(documentClient, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.Countries);
            });
            services.AddSingleton<IUpdateRepository<Country>>(serviceProvider => {
                var documentClient = serviceProvider.GetService<IDocumentClient>();
                return new UpdateRepository<Country>(documentClient, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.Countries);
            });
            #endregion (Country)

            #region Image
            services.AddSingleton<ICreateRepository<Image>>(serviceProvider => {
                var documentClient = serviceProvider.GetService<IDocumentClient>();
                return new CreateRepository<Image>(documentClient, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.Images);
            });
            #endregion (Image)

            #region Dashboard
            services.AddSingleton<IGetRepository<Dashboard<IEnumerable<CountBy<RefValue>>>>>(serviceProvider => {
                var documentClient = serviceProvider.GetService<IDocumentClient>();
                return new GetRepository<Dashboard<IEnumerable<CountBy<RefValue>>>>(documentClient, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.Statistics);
            });
            services.AddSingleton<IGetRepository<Dashboard<IEnumerable<CountBy<NodaTime.LocalDate>>>>>(serviceProvider => {
                var documentClient = serviceProvider.GetService<IDocumentClient>();
                return new GetRepository<Dashboard<IEnumerable<CountBy<NodaTime.LocalDate>>>>(documentClient, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.Statistics);
            });
            services.AddSingleton<IUpsertRepository<Dashboard<IEnumerable<CountBy<RefValue>>>>>(serviceProvider => {
                var documentClient = serviceProvider.GetService<IDocumentClient>();
                return new UpsertRepository<Dashboard<IEnumerable<CountBy<RefValue>>>>(documentClient, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.Statistics);
            });
            services.AddSingleton<IUpsertRepository<Dashboard<IEnumerable<CountBy<NodaTime.LocalDate>>>>>(serviceProvider => {
                var documentClient = serviceProvider.GetService<IDocumentClient>();
                return new UpsertRepository<Dashboard<IEnumerable<CountBy<NodaTime.LocalDate>>>>(documentClient, DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.Statistics);
            });
            #endregion (Dashboard)

            return services;
        }

        public static IServiceCollection AddQueries(this IServiceCollection services) {
            #region Bag
            services.AddSingleton<SearchQueryHandler<Application.Services.ViewModels.Tea.Bag, Domain.Tea.Bag>, SearchQueryHandler<Application.Services.ViewModels.Tea.Bag, Domain.Tea.Bag>>();
            services.AddSingleton<GetQueryHandler<Application.Services.ViewModels.Tea.Bag, Domain.Tea.Bag>, GetQueryHandler<Application.Services.ViewModels.Tea.Bag, Domain.Tea.Bag>>();
            #endregion (Bag)

            #region BagType
            services.AddSingleton<SearchQueryHandler<Application.Services.ViewModels.Tea.BagType, Domain.Tea.BagType>, SearchQueryHandler<Application.Services.ViewModels.Tea.BagType, Domain.Tea.BagType>>();
            services.AddSingleton<GetQueryHandler<Application.Services.ViewModels.Tea.BagType, Domain.Tea.BagType>, GetQueryHandler<Application.Services.ViewModels.Tea.BagType, Domain.Tea.BagType>>();
            services.AddSingleton<IAsyncQueryHandler<SearchRefValuesQuery<Domain.Tea.BagType>>, SearchRefValuesQueryHandler<Domain.Tea.BagType>>();
            #endregion (BagType)

            #region Brands
            services.AddSingleton<SearchQueryHandler<Application.Services.ViewModels.Tea.Brand, Domain.Tea.Brand>, SearchQueryHandler<Application.Services.ViewModels.Tea.Brand, Domain.Tea.Brand>>();
            services.AddSingleton<GetQueryHandler<Application.Services.ViewModels.Tea.Brand, Domain.Tea.Brand>, GetQueryHandler<Application.Services.ViewModels.Tea.Brand, Domain.Tea.Brand>>();
            services.AddSingleton<IAsyncQueryHandler<SearchRefValuesQuery<Domain.Tea.Brand>>, SearchRefValuesQueryHandler<Domain.Tea.Brand>>();
            #endregion (Brands)

            #region Country
            services.AddSingleton<SearchQueryHandler<Application.Services.ViewModels.Tea.Country, Domain.Tea.Country>, SearchQueryHandler<Application.Services.ViewModels.Tea.Country, Domain.Tea.Country>>();
            services.AddSingleton<GetQueryHandler<Application.Services.ViewModels.Tea.Country, Domain.Tea.Country>, GetQueryHandler<Application.Services.ViewModels.Tea.Country, Domain.Tea.Country>>();
            services.AddSingleton<IAsyncQueryHandler<SearchRefValuesQuery<Domain.Tea.Country>>, SearchRefValuesQueryHandler<Domain.Tea.Country>>();
            #endregion (Country)

            #region Dashboard
            services.AddSingleton<IAsyncQueryHandler<TotalBagsCountByInsertDateQuery>, TotalBagsCountByInsertDateQueryHandler>();
            services.AddSingleton<IAsyncQueryHandler<BagsCountByInsertDateQuery>, BagsCountByInsertDateQueryHandler>();
            services.AddSingleton<IAsyncQueryHandler<BagsCountByBrandsQuery>, BagsCountByBrandsQueryHandler>();
            services.AddSingleton<IAsyncQueryHandler<BagsCountByBagTypesQuery>, BagsCountByBagTypesQueryHandler>();
            #endregion (Dashboard)
            return services;
        }

        public static IServiceCollection AddCommands(this IServiceCollection services) {
            #region Bag
            services.AddSingleton<IAsyncCommandHandler<UpdateBagCommand>, UpdateBagCommandHandler>();
            services.AddSingleton<IAsyncCommandHandler<CreateBagCommand>, CreateBagCommandHandler>();
            #endregion (Bag)

            #region BagType
            services.AddSingleton<IAsyncCommandHandler<UpdateCommand<Application.Services.ViewModels.Tea.BagType>, Domain.Tea.BagType>, UpdateCommandHandler<Application.Services.ViewModels.Tea.BagType, Domain.Tea.BagType>>();
            services.AddSingleton<IAsyncCommandHandler<CreateCommand<Application.Services.ViewModels.Tea.BagType>, Domain.Tea.BagType>, CreateCommandHandler<Application.Services.ViewModels.Tea.BagType, Domain.Tea.BagType>>();
            #endregion (BagType)

            #region Brands
            services.AddSingleton<IAsyncCommandHandler<UpdateCommand<Application.Services.ViewModels.Tea.Brand>, Domain.Tea.Brand>, UpdateCommandHandler<Application.Services.ViewModels.Tea.Brand, Domain.Tea.Brand>>();
            services.AddSingleton<IAsyncCommandHandler<CreateCommand<Application.Services.ViewModels.Tea.Brand>, Domain.Tea.Brand>, CreateCommandHandler<Application.Services.ViewModels.Tea.Brand, Domain.Tea.Brand>>();
            #endregion (Brands)

            #region Country
            services.AddSingleton<IAsyncCommandHandler<UpdateCommand<Application.Services.ViewModels.Tea.Country>, Domain.Tea.Country>, UpdateCommandHandler<Application.Services.ViewModels.Tea.Country, Domain.Tea.Country>>();
            services.AddSingleton<IAsyncCommandHandler<CreateCommand<Application.Services.ViewModels.Tea.Country>, Domain.Tea.Country>, CreateCommandHandler<Application.Services.ViewModels.Tea.Country, Domain.Tea.Country>>();
            #endregion (Country)

            #region Dashboard
            services.AddSingleton<IAsyncCommandHandler<CreateBagsCountByBagTypesCommand>, CreateBagsCountByBagTypesCommandHandler>();
            services.AddSingleton<IAsyncCommandHandler<CreateBagsCountByBrandsCommand>, CreateBagsCountByBrandsCommandHandler>();
            services.AddSingleton<IAsyncCommandHandler<CreateBagsCountByInsertDateCommand>, CreateBagsCountByInsertDateCommandHandler>();
            services.AddSingleton<IAsyncCommandHandler<CreateTotalBagsCountByInsertDateCommand>, CreateTotalBagsCountByInsertDateCommandHandler>();
            #endregion (Dashboard)

            services.AddSingleton<IAsyncCommandHandler<UploadTeabagImageCommand>, UploadTeabagImageCommandHandler>();
            return services;
        }

        public static IServiceCollection AddNotificationServices(this IServiceCollection services) {
            return services;
        }

        public static IServiceCollection AddSchedulingServices(this IServiceCollection services) {
            services.AddSingleton<IScheduledTask, UpdateBagsCountByBagTypeStatisticsTask>();
            services.AddSingleton<IScheduledTask, UpdateCountByBrandStatistcsTask>();
            services.AddSingleton<IScheduledTask, UpdateBagsCountByInsertDateStatisticsTask>();
            services.AddSingleton<IScheduledTask, UpdateTotalCountByInsertDateStatisticsTask>();
            services.AddSingleton<IHostedService, SchedulerHostedService>(serviceProvider => {
                var logger = serviceProvider.GetService<ILogger<SchedulerHostedService>>();
                var clock = serviceProvider.GetService<IClock>();
                var tasks = serviceProvider.GetServices<IScheduledTask>();
                var instance = new SchedulerHostedService(tasks.Select(task => new Scheduler(task, clock)));
                instance.UnobservedTaskException += (sender, args) => {
                    // log this
                    logger.LogErrorAsync($"{nameof(SchedulerHostedService.UnobservedTaskException)}: {args}").GetAwaiter().GetResult();
                    args.SetObserved();
                };
                return instance;
            });

            return services;
        }
    }
}
