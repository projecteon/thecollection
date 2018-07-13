namespace TheCollection.Presentation.Web {
    using System;
    using System.Text.RegularExpressions;
    using AspNetCore.Identity.DocumentDb;
    using AspNetCore.Identity.DocumentDb.Tools;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SpaServices.Webpack;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using Newtonsoft.Json.Serialization;
    using NodaTime;
    using NodaTime.Serialization.JsonNet;
    using Swashbuckle.NodaTime.AspNetCore;
    using Swashbuckle.AspNetCore.Swagger;
    using TheCollection.Application.Services;
    using TheCollection.Data.DocumentDB.Extensions;
    using TheCollection.Domain.Core.Contracts.Repository;
    using TheCollection.Domain.Extensions;
    using TheCollection.Presentation.Web.Constants;
    using TheCollection.Presentation.Web.Controllers;
    using TheCollection.Presentation.Web.Extensions;
    using TheCollection.Presentation.Web.Handlers;
    using TheCollection.Presentation.Web.Models;
    using TheCollection.Presentation.Web.Repositories;
    using Microsoft.AspNetCore.Mvc.Infrastructure;
    using Microsoft.AspNetCore.Mvc.Routing;
  using Microsoft.AspNetCore.Http;

  public class Startup {
        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services) {
            // wire application
            services.WireDependencies();

            services.AddSingleton<IDocumentClient>(InitializeDocumentClient(
                Configuration.GetValue<Uri>("DocumentDbClient:EndpointUri"),
                Configuration.GetValue<string>("DocumentDbClient:AuthorizationKey"))
            );

            // Add framework services.
            // consider: https://github.com/imranbaloch/ASPNETIdentityWithOnion
            services.AddIdentity<WebUser, WebRole>()
            .AddDocumentDbStores(options => {
                options.UserStoreDocumentCollection = DocumentDBConstants.Collections.AspNetIdentity;
                options.RoleStoreDocumentCollection = DocumentDBConstants.Collections.AspNetIdentity;
                options.Database = DocumentDBConstants.DatabaseId;
            })
            .AddDefaultTokenProviders();

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

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
                options.ClientId = Configuration.GetValue<string>("OAuth:Google:ClientId");
                options.ClientSecret = Configuration.GetValue<string>("OAuth:Google:ClientSecret");
            })
            .AddFacebook(options => {
                options.AppId = Configuration.GetValue<string>("OAuth:Facebook:ClientId");
                options.AppSecret = Configuration.GetValue<string>("OAuth:Facebook:ClientSecret");
            })
            .AddMicrosoftAccount(options => {
                options.ClientId = Configuration.GetValue<string>("OAuth:Microsoft:ClientId");
                options.ClientSecret = Configuration.GetValue<string>("OAuth:Microsoft:ClientSecret");
            });

            services.AddSingleton<IImageRepository, ImageFilesystemRepository>();
            //services.AddSingleton<IImageRepository>(x => new ImageAzureBlobRepository(Configuration.GetValue<string>("StorageAccount:Scheme"),
            //                                                                    Configuration.GetValue<string>("StorageAccount:Name"),
            //                                                                    Configuration.GetValue<string>("StorageAccount:Key"),
            //                                                                    Configuration.GetValue<string>("StorageAccount:Endpoints"))
            //);

            // Add framework services.
            services.AddMvc(
                options => {
                    options.SslPort = 44330;
                    options.Filters.Add(new RequireHttpsAttribute());
            })
            .AddJsonOptions(options => { options.SerializerSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb); })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info { Title = "MeetingRoom API", Version = "v1" });

                var settings = new JsonSerializerSettings {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Converters = {
                      new StringEnumConverter()
                    },
                    NullValueHandling = NullValueHandling.Ignore
                };
                settings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
                c.ConfigureForNodaTime(settings);
            });

            services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 44330;
            });

            // Build the intermediate service provider then return it
            return services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(Microsoft.AspNetCore.Builder.IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider) {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                //app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {
                //    HotModuleReplacement = true,
                //    HotModuleReplacementEndpoint = "/dist/__webpack_hmr",
                //    ReactHotModuleReplacement = true
                //});
            }
            else {
                app.UseExceptionHandler("/Home/Error");
                //app.UseHsts();
            }

            // Create branch to the MyHandlerMiddleware.
            app.MapWhen(
                context => Regex.IsMatch(context.Request.Path.ToString(), ThumbnailHandler.RegEx),
                appBranch => { appBranch.UseThumbnailHandler(); }
            );

            app.MapWhen(
                context => Regex.IsMatch(context.Request.Path.ToString(), ImageHandler.RegEx),
                appBranch => { appBranch.UseImageHandler(); }
            );

            // app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MeetingRoom API V1");
            });

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "account",
                    template: "{controller=Account}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });

            CreateRoles(serviceProvider);
        }

        DocumentClient InitializeDocumentClient(Uri endpointUri, string authorizationKey, JsonSerializerSettings serializerSettings = null) {
            serializerSettings = serializerSettings ?? new JsonSerializerSettings();
            serializerSettings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
            serializerSettings.Converters.Add(new JsonClaimConverter());
            serializerSettings.Converters.Add(new TheCollection.Presentation.Web.JsonClaimsPrincipalConverter());
            serializerSettings.Converters.Add(new TheCollection.Presentation.Web.JsonClaimsIdentityConverter());
            // Create a DocumentClient and an initial collection (if it does not exist yet) for sample purposes
            var client = new DocumentClient(endpointUri, authorizationKey, serializerSettings, new ConnectionPolicy { EnableEndpointDiscovery = false }, null);
            client.CreateCollectionIfNotExistsAsync(DocumentDBConstants.DatabaseId, DocumentDBConstants.Collections.AspNetIdentity).Wait();
            return client;
        }

        // https://msdn.microsoft.com/en-us/magazine/mt826337.aspx
        // https://stackoverflow.com/questions/44180773/dependency-injection-in-asp-net-core-2-throws-exception
        void CreateRoles(IServiceProvider serviceProvider) {
            //initializing custom roles
            using (var scope = serviceProvider.CreateScope()) {
                var RoleManager = serviceProvider.GetRequiredService<RoleManager<WebRole>>();
                var UserManager = serviceProvider.GetRequiredService<UserManager<WebUser>>();
                string[] roleNames = { Roles.SystemAdministrator, Roles.TeaManager, Roles.Collector, Roles.Member };
                IdentityResult roleResult;

                foreach (var roleName in roleNames) {
                    var roleExist = RoleManager.RoleExistsAsync(roleName).Result;
                    if (!roleExist) {
                        //create the roles and seed them to the database: Question 1
                        roleResult = RoleManager.CreateAsync(new WebRole { Name = roleName }).Result;
                    }
                }

                var _user = UserManager.FindByEmailAsync("gledesrus@hotmail.com").Result;
                if (_user != null && _user.Roles.None(x => x.Name != Roles.SystemAdministrator)) {
                    roleResult = UserManager.AddToRoleAsync(_user, Roles.SystemAdministrator).Result;
                }

                _user = UserManager.FindByEmailAsync("l.wolterink@hotmail.com").Result;
                if (_user != null && _user.Roles.None(x => x.Name != Roles.TeaManager)) {
                    roleResult = UserManager.AddToRoleAsync(_user, Roles.TeaManager).Result;
                }
            }
        }
    }
}
