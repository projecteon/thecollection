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

  public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = { new StringEnumConverter() },
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            }.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            // wire application
            services.WireDependencies(Configuration);
            services.AddLoginIdentities(Configuration);

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

            services.AddSwagger();

            services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 44330;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(Microsoft.AspNetCore.Builder.IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider, RoleManager<WebRole> roleManager, UserManager<WebUser> userManager) {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {
                    HotModuleReplacement = true,
                    //HotModuleReplacementEndpoint = "/dist/__webpack_hmr",
                    ReactHotModuleReplacement = true
                });
            }
            else {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Add custom middleweara for thumbnails and images.
            app.UseThumbnailHandler();
            app.UseImageHandler();

            // app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwagger();

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

            CreateRoles(roleManager, userManager);
        }

        // https://msdn.microsoft.com/en-us/magazine/mt826337.aspx
        // https://stackoverflow.com/questions/44180773/dependency-injection-in-asp-net-core-2-throws-exception
        void CreateRoles(RoleManager<WebRole> roleManager, UserManager<WebUser> userManager) {
            //initializing custom roles
            string[] roleNames = { Roles.SystemAdministrator, Roles.TeaManager, Roles.Collector, Roles.Member };
            IdentityResult roleResult;

            foreach (var roleName in roleNames) {
                var roleExist = roleManager.RoleExistsAsync(roleName).Result;
                if (!roleExist) {
                    //create the roles and seed them to the database: Question 1
                    roleResult = roleManager.CreateAsync(new WebRole { Name = roleName }).Result;
                }
            }

            var _user = userManager.FindByEmailAsync("gledesrus@hotmail.com").Result;
            if (_user != null && _user.Roles.None(x => x.Name == Roles.SystemAdministrator)) {
                roleResult = userManager.AddToRoleAsync(_user, Roles.SystemAdministrator).Result;
            }

            _user = userManager.FindByEmailAsync("l.wolterink@hotmail.com").Result;
            if (_user != null && _user.Roles.None(x => x.Name == Roles.TeaManager)) {
                roleResult = userManager.AddToRoleAsync(_user, Roles.TeaManager).Result;
            }
        }
    }
}
