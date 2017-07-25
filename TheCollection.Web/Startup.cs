using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Documents.Client;
using TheCollection.Web.Services;
using System.Text.RegularExpressions;
using TheCollection.Web.Handlers;
using Microsoft.Azure.Documents;
using System.Net;
using AspNetCore.Identity.DocumentDb;
using System.IO;
using TheCollection.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace TheCollection_Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddSingleton<DocumentClient>(x => new DocumentClient(
            //    Configuration.GetValue<Uri>("DocumentDbClient:EndpointUri"),
            //    Configuration.GetValue<string>("DocumentDbClient:AuthorizationKey")
            //));

            services.AddSingleton<IDocumentClient>(InitializeDocumentClient(
                Configuration.GetValue<Uri>("DocumentDbClient:EndpointUri"),
                Configuration.GetValue<string>("DocumentDbClient:AuthorizationKey")));

            // Add framework services.
            services.AddIdentity<ApplicationUser, DocumentDbIdentityRole>(options =>
            {
                options.Cookies.ApplicationCookie.AuthenticationScheme = "ApplicationCookie";
                options.Cookies.ApplicationCookie.CookieName = "Interop";
                options.Cookies.ApplicationCookie.DataProtectionProvider = DataProtectionProvider.Create(new DirectoryInfo("C:\\Github\\Identity\\artifacts"));
            })
            .AddDocumentDbStores(options =>
            {
                options.UserStoreDocumentCollection = "AspNetIdentity";
                options.Database = "TheCollection";
            })
            .AddDefaultTokenProviders();

            //services.AddSingleton<IImageService, ImageFilesystemService>();
            services.AddSingleton<IImageService>(x => new ImageAzureBlobService($"DefaultEndpointsProtocol={Configuration.GetValue<string>("StorageAccount:Scheme")};AccountName={Configuration.GetValue<string>("StorageAccount:Name")};AccountKey={Configuration.GetValue<string>("StorageAccount:Key")};{Configuration.GetValue<string>("StorageAccount:Endpoints")}"));

            // Add framework services.
            services.AddMvc(
                options =>
                {
                    options.SslPort = 44330;
                    options.Filters.Add(new RequireHttpsAttribute());
                }
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {
                    HotModuleReplacement = true,
                    ReactHotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // Create branch to the MyHandlerMiddleware. 
            // All requests ending in .report will follow this branch.
            app.MapWhen(
                context => Regex.IsMatch(context.Request.Path.ToString(), ThumbnailHandler.RegEx),
                appBranch => { appBranch.UseThumbnailHandler(); }
            );

            app.MapWhen(
                context => Regex.IsMatch(context.Request.Path.ToString(), ImageHandler.RegEx),
                appBranch => { appBranch.UseImageHandler(); }
            );

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715
            // https://docs.microsoft.com/en-gb/aspnet/core/security/authentication/social/index
            app.UseGoogleAuthentication(new GoogleOptions()
            {
                ClientId = "229435238043-uudbjbi8e0tul6evho5vk8lkmck6r1rc.apps.googleusercontent.com",
                ClientSecret = "4te-tqjVO2VvvTwTudoKXq-s"
            });

            app.UseFacebookAuthentication(new FacebookOptions()
            {
                ClientId = "652967514908861",
                ClientSecret = "8cf4ab15da3083db31e313864458b616"
            });

            app.UseMicrosoftAccountAuthentication(new MicrosoftAccountOptions()
            {
                ClientId = "e75794ef-c77a-446a-a260-d03d45f361d5",
                ClientSecret = "EBo9NqAJSRPKcAtrdX52ZxC"
            });

            app.UseMvc(routes =>
            {
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
        }

        private DocumentClient InitializeDocumentClient(Uri endpointUri, string authorizationKey)
        {
            // Create a DocumentClient and an initial collection (if it does not exist yet) for sample purposes
            DocumentClient client = new DocumentClient(endpointUri, authorizationKey, new ConnectionPolicy { EnableEndpointDiscovery = false });

            try
            {
                // Does the DB exist?
                var db = client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri("TheCollection")).Result;
            }
            catch (AggregateException ae)
            {
                ae.Handle(ex =>
                {
                    if (ex.GetType() == typeof(DocumentClientException) && ((DocumentClientException)ex).StatusCode == HttpStatusCode.NotFound)
                    {
                        // Create DB
                        var db = client.CreateDatabaseAsync(new Database() { Id = "TheCollection" }).Result;
                        return true;
                    }

                    return false;
                });
            }

            try
            {
                // Does the Collection exist?
                var collection = client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri("TheCollection", "AspNetIdentity")).Result;
            }
            catch (AggregateException ae)
            {
                ae.Handle(ex =>
                {
                    if (ex.GetType() == typeof(DocumentClientException) && ((DocumentClientException)ex).StatusCode == HttpStatusCode.NotFound)
                    {
                        DocumentCollection collection = new DocumentCollection() { Id = "AspNetIdentity" };
                        collection = client.CreateDocumentCollectionAsync(UriFactory.CreateDatabaseUri("TheCollection"), collection).Result;

                        return true;
                    }

                    return false;
                });
            }

            return client;
        }
    }
}
