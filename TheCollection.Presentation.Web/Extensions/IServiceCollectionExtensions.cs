namespace TheCollection.Presentation.Web.Extensions {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AspNetCore.Identity.DocumentDb;
    using AspNetCore.Identity.DocumentDb.Tools;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.Routing;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;
    using NodaTime;
    using NodaTime.Serialization.JsonNet;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.NodaTime.AspNetCore;
    using TheCollection.Api;
    using TheCollection.Application.Services;
    using TheCollection.Application.Services.Commands;
    using TheCollection.Application.Services.Commands.Tea;
    using TheCollection.Application.Services.Contracts;
    using TheCollection.Application.Services.Queries;
    using TheCollection.Application.Services.Queries.Tea;
    using TheCollection.Application.Services.Translators;
    using TheCollection.Application.Services.Translators.Tea;
    using TheCollection.Data.DocumentDB.Extensions;
    using TheCollection.Data.DocumentDB.Repositories;
    using TheCollection.Domain;
    using TheCollection.Domain.Core.Contracts;
    using TheCollection.Domain.Core.Contracts.Repository;
    using TheCollection.Domain.Tea;
    using TheCollection.Infrastructure.Logging;
    using TheCollection.Infrastructure.Scheduling;
    using TheCollection.Infrastructure.Scheduling.Tea;
    using TheCollection.Presentation.Web.Constants;
    using TheCollection.Presentation.Web.Controllers;
    using TheCollection.Presentation.Web.Models;
    using TheCollection.Presentation.Web.Repositories;

    public static class IServiceCollectionExtensions {
        public static IServiceCollection WireDependencies(this IServiceCollection services, IConfiguration configuration) {
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(it =>
                it.GetRequiredService<IUrlHelperFactory>()
                    .GetUrlHelper(it.GetRequiredService<IActionContextAccessor>().ActionContext)
            );

            services.AddSingleton<IDocumentClient>(InitializeDocumentClient(
                configuration.GetValue<Uri>("DocumentDbClient:EndpointUri"),
                configuration.GetValue<string>("DocumentDbClient:AuthorizationKey"))
            );


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
            services.AddScoped<IAsyncTranslator<Domain.RefValue, Application.Services.ViewModels.RefValue>, RefValueToRefValueTranslator>();
            services.AddScoped<IAsyncTranslator<Domain.Tea.Bag, Application.Services.ViewModels.Tea.Bag>, BagToBagViewModelTranslator>();
            services.AddScoped<IAsyncTranslator<Domain.Tea.BagType, Application.Services.ViewModels.Tea.BagType>, BagTypeToBagTypeTranslator>();
            services.AddScoped<IAsyncTranslator<Domain.Tea.Brand, Application.Services.ViewModels.Tea.Brand>, BrandToBrandTranslator>();
            services.AddScoped<IAsyncTranslator<Domain.Tea.Country, Application.Services.ViewModels.Tea.Country>, CountryToCountryViewModelTranslator>();
            #endregion (Domain models to view models)

            #region View models to domain models
            services.AddScoped<ITranslator<Application.Services.ViewModels.RefValue, Domain.RefValue>, RefValueDtoToRefValueTranslator>();
            services.AddScoped<ITranslator<Application.Services.ViewModels.Tea.Bag, Domain.Tea.Bag, Domain.Tea.Bag>, BagViewModelToUpdateBagTranslator>();
            services.AddScoped<IAsyncTranslator<Application.Services.ViewModels.Tea.Bag, Domain.Tea.Bag>, BagViewModelToCreateBagTranslator>();
            services.AddScoped<ITranslator<Application.Services.ViewModels.Tea.BagType, Domain.Tea.BagType>, BagTypeDtoToBagTypeTranslator>();
            services.AddScoped<ITranslator<Application.Services.ViewModels.Tea.Brand, Domain.Tea.Brand>, BrandDtoToBrandTranslator>();
            services.AddScoped<ITranslator<Application.Services.ViewModels.Tea.Country, Domain.Tea.Country>, CountryViewModelToCountryTranslator>();
            #endregion (View models to domain models)

            services.AddScoped<ITranslator<ICommandResult, IActionResult>, ICommandResultToIActionResultTranslator>();
            services.AddScoped<ITranslator<IQueryResult, IActionResult>, IQueryResultToIActionResultTranslator>();
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services) {
            services.AddScoped<IGetRepository<IApplicationUser>, WebUserRepository>();
            services.AddScoped<IGetAllRepository<IApplicationUser>, WebUserRepository>();

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
            services.AddScoped<SearchQueryHandler<Application.Services.ViewModels.Tea.Bag, Domain.Tea.Bag>, SearchQueryHandler<Application.Services.ViewModels.Tea.Bag, Domain.Tea.Bag>>();
            services.AddScoped<GetQueryHandler<Application.Services.ViewModels.Tea.Bag, Domain.Tea.Bag>, GetQueryHandler<Application.Services.ViewModels.Tea.Bag, Domain.Tea.Bag>>();
            #endregion (Bag)

            #region BagType
            services.AddScoped<SearchQueryHandler<Application.Services.ViewModels.Tea.BagType, Domain.Tea.BagType>, SearchQueryHandler<Application.Services.ViewModels.Tea.BagType, Domain.Tea.BagType>>();
            services.AddScoped<GetQueryHandler<Application.Services.ViewModels.Tea.BagType, Domain.Tea.BagType>, GetQueryHandler<Application.Services.ViewModels.Tea.BagType, Domain.Tea.BagType>>();
            services.AddScoped<IAsyncQueryHandler<SearchRefValuesQuery<Domain.Tea.BagType>>, SearchRefValuesQueryHandler<Domain.Tea.BagType>>();
            #endregion (BagType)

            #region Brands
            services.AddScoped<SearchQueryHandler<Application.Services.ViewModels.Tea.Brand, Domain.Tea.Brand>, SearchQueryHandler<Application.Services.ViewModels.Tea.Brand, Domain.Tea.Brand>>();
            services.AddScoped<GetQueryHandler<Application.Services.ViewModels.Tea.Brand, Domain.Tea.Brand>, GetQueryHandler<Application.Services.ViewModels.Tea.Brand, Domain.Tea.Brand>>();
            services.AddScoped<IAsyncQueryHandler<SearchRefValuesQuery<Domain.Tea.Brand>>, SearchRefValuesQueryHandler<Domain.Tea.Brand>>();
            #endregion (Brands)

            #region Country
            services.AddScoped<SearchQueryHandler<Application.Services.ViewModels.Tea.Country, Domain.Tea.Country>, SearchQueryHandler<Application.Services.ViewModels.Tea.Country, Domain.Tea.Country>>();
            services.AddScoped<GetQueryHandler<Application.Services.ViewModels.Tea.Country, Domain.Tea.Country>, GetQueryHandler<Application.Services.ViewModels.Tea.Country, Domain.Tea.Country>>();
            services.AddScoped<IAsyncQueryHandler<SearchRefValuesQuery<Domain.Tea.Country>>, SearchRefValuesQueryHandler<Domain.Tea.Country>>();
            #endregion (Country)

            #region Dashboard
            services.AddScoped<IAsyncQueryHandler<TotalBagsCountByInsertDateQuery>, TotalBagsCountByInsertDateQueryHandler>();
            services.AddScoped<IAsyncQueryHandler<BagsCountByInsertDateQuery>, BagsCountByInsertDateQueryHandler>();
            services.AddScoped<IAsyncQueryHandler<BagsCountByBrandsQuery>, BagsCountByBrandsQueryHandler>();
            services.AddScoped<IAsyncQueryHandler<BagsCountByBagTypesQuery>, BagsCountByBagTypesQueryHandler>();
            #endregion (Dashboard)
            return services;
        }

        public static IServiceCollection AddCommands(this IServiceCollection services) {
            #region Bag
            services.AddScoped<IAsyncCommandHandler<UpdateBagCommand>, UpdateBagCommandHandler>();
            services.AddScoped<IAsyncCommandHandler<CreateBagCommand>, CreateBagCommandHandler>();
            #endregion (Bag)

            #region BagType
            services.AddScoped<IAsyncCommandHandler<UpdateCommand<Application.Services.ViewModels.Tea.BagType>, Domain.Tea.BagType>, UpdateCommandHandler<Application.Services.ViewModels.Tea.BagType, Domain.Tea.BagType>>();
            services.AddScoped<IAsyncCommandHandler<CreateCommand<Application.Services.ViewModels.Tea.BagType>, Domain.Tea.BagType>, CreateCommandHandler<Application.Services.ViewModels.Tea.BagType, Domain.Tea.BagType>>();
            #endregion (BagType)

            #region Brands
            services.AddScoped<IAsyncCommandHandler<UpdateCommand<Application.Services.ViewModels.Tea.Brand>, Domain.Tea.Brand>, UpdateCommandHandler<Application.Services.ViewModels.Tea.Brand, Domain.Tea.Brand>>();
            services.AddScoped<IAsyncCommandHandler<CreateCommand<Application.Services.ViewModels.Tea.Brand>, Domain.Tea.Brand>, CreateCommandHandler<Application.Services.ViewModels.Tea.Brand, Domain.Tea.Brand>>();
            #endregion (Brands)

            #region Country
            services.AddScoped<IAsyncCommandHandler<UpdateCommand<Application.Services.ViewModels.Tea.Country>, Domain.Tea.Country>, UpdateCommandHandler<Application.Services.ViewModels.Tea.Country, Domain.Tea.Country>>();
            services.AddScoped<IAsyncCommandHandler<CreateCommand<Application.Services.ViewModels.Tea.Country>, Domain.Tea.Country>, CreateCommandHandler<Application.Services.ViewModels.Tea.Country, Domain.Tea.Country>>();
            #endregion (Country)

            #region Dashboard
            services.AddScoped<IAsyncCommandHandler<CreateBagsCountByBagTypesCommand>, CreateBagsCountByBagTypesCommandHandler>();
            services.AddScoped<IAsyncCommandHandler<CreateBagsCountByBrandsCommand>, CreateBagsCountByBrandsCommandHandler>();
            services.AddScoped<IAsyncCommandHandler<CreateBagsCountByInsertDateCommand>, CreateBagsCountByInsertDateCommandHandler>();
            services.AddScoped<IAsyncCommandHandler<CreateTotalBagsCountByInsertDateCommand>, CreateTotalBagsCountByInsertDateCommandHandler>();
            #endregion (Dashboard)

            services.AddScoped<IAsyncCommandHandler<UploadTeabagImageCommand>, UploadTeabagImageCommandHandler>();
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

        public static IServiceCollection AddSwagger(this IServiceCollection services) {
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info { Title = "TheCollection API", Version = "v1" });

                var settings = new JsonSerializerSettings {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Converters = {
                       new StringEnumConverter()
                     },
                    NullValueHandling = NullValueHandling.Ignore
                }.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);

                c.ConfigureForNodaTime(settings);
            });
            return services;
        }

        public static IServiceCollection AddLoginIdentities(this IServiceCollection services, IConfiguration configuration) {

            // Add framework services.
            // consider: https://github.com/imranbaloch/ASPNETIdentityWithOnion
            services.AddIdentity<WebUser, WebRole>()
            .AddDocumentDbStores(options => {
                options.UserStoreDocumentCollection = DocumentDBConstants.Collections.AspNetIdentity;
                options.RoleStoreDocumentCollection = DocumentDBConstants.Collections.AspNetIdentity;
                options.Database = DocumentDBConstants.DatabaseId;
            })
            .AddDefaultTokenProviders();

            services.Configure<CookiePolicyOptions>(options => {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.ConfigureApplicationCookie(options => {
                //options.DataProtectionProvider = DataProtectionProvider.Create(new DirectoryInfo("C:\\TheCollection\\Identity\\artifacts"));
                options.LoginPath = $"/Account/{nameof(AccountController.Login)}";
                options.LogoutPath = $"/Account/{nameof(AccountController.LogOff)}";
            });

            // Add external authentication middleware below.
            // To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715
            // https://docs.microsoft.com/en-gb/aspnet/core/security/authentication/social/index
            // https://docs.microsoft.com/en-us/aspnet/core/migration/1x-to-2x/identity-2x
            services.AddAuthentication()
            .AddGoogle(options => {
                options.ClientId = configuration.GetValue<string>("OAuth:Google:ClientId");
                options.ClientSecret = configuration.GetValue<string>("OAuth:Google:ClientSecret");
            })
            .AddFacebook(options => {
                options.AppId = configuration.GetValue<string>("OAuth:Facebook:ClientId");
                options.AppSecret = configuration.GetValue<string>("OAuth:Facebook:ClientSecret");
            })
            .AddMicrosoftAccount(options => {
                options.ClientId = configuration.GetValue<string>("OAuth:Microsoft:ClientId");
                options.ClientSecret = configuration.GetValue<string>("OAuth:Microsoft:ClientSecret");
            });

            return services;
        }

        static DocumentClient InitializeDocumentClient(Uri endpointUri, string authorizationKey, JsonSerializerSettings serializerSettings = null) {
            serializerSettings = serializerSettings ?? new JsonSerializerSettings();
            serializerSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
            serializerSettings.Converters.Add(new JsonClaimConverter());
            serializerSettings.Converters.Add(new TheCollection.Presentation.Web.JsonClaimsPrincipalConverter());
            serializerSettings.Converters.Add(new TheCollection.Presentation.Web.JsonClaimsIdentityConverter());
            // Create a DocumentClient and an initial collection (if it does not exist yet) for sample purposes
            var client = new DocumentClient(endpointUri, authorizationKey, serializerSettings, new ConnectionPolicy { EnableEndpointDiscovery = false }, null);
            client.CreateCollectionIfNotExistsAsync(DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.AspNetIdentity).Wait(); // handle in application start Configure(IApplicationBuilder) or use IHostedService
            return client;
        }
    }
}
