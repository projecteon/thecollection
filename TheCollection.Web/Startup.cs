namespace TheCollection_Web {

    using System;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using AspNetCore.Identity.DocumentDb;
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
    using TheCollection.Web.Constants;
    using TheCollection.Web.Controllers;
    using TheCollection.Web.Extensions;
    using TheCollection.Web.Handlers;
    using TheCollection.Web.Models;
    using TheCollection.Web.Services;

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
        public void ConfigureServices(IServiceCollection services) {
            services.AddScoped<IApplicationUserAccessor, ApplicationUserAccessor>();

            services.AddSingleton<IDocumentClient>(InitializeDocumentClient(
                Configuration.GetValue<Uri>("DocumentDbClient:EndpointUri"),
                Configuration.GetValue<string>("DocumentDbClient:AuthorizationKey"))
            );

            // Add framework services.
            services.AddIdentity<ApplicationUser, DocumentDbIdentityRole>(options => {
                options.Cookies.ApplicationCookie.AuthenticationScheme = "ApplicationCookie";
                options.Cookies.ApplicationCookie.AutomaticAuthenticate = true;
                options.Cookies.ApplicationCookie.AutomaticChallenge = true;
                options.Cookies.ApplicationCookie.CookieName = "Interop";
                //options.Cookies.ApplicationCookie.DataProtectionProvider = DataProtectionProvider.Create(new DirectoryInfo("C:\\TheCollection\\Identity\\artifacts"));
                options.Cookies.ApplicationCookie.LoginPath = $"/Account/{nameof(AccountController.Login)}";
                options.Cookies.ApplicationCookie.LogoutPath = $"/Account/{nameof(AccountController.LogOff)}";
            })
            .AddDocumentDbStores(options => {
                options.UserStoreDocumentCollection = DocumentDB.AspNetIdentity;
                options.RoleStoreDocumentCollection = DocumentDB.AspNetIdentityRoles;
                options.Database = DocumentDB.DatabaseId;
            })
            .AddDefaultTokenProviders();

            //services.AddSingleton<IImageService, ImageFilesystemService>();
            services.AddSingleton<IImageService>(x => new ImageAzureBlobService(Configuration.GetValue<string>("StorageAccount:Scheme"),
                                                                                Configuration.GetValue<string>("StorageAccount:Name"),
                                                                                Configuration.GetValue<string>("StorageAccount:Key"),
                                                                                Configuration.GetValue<string>("StorageAccount:Endpoints"))
            );

            // wire repositories
            // services.AddScoped(typeof(ISearchRepository<>), typeof(SearchRepository<>));

            // Add framework services.
            services.AddMvc(
                options => {
                    options.SslPort = 44330;
                    options.Filters.Add(new RequireHttpsAttribute());
                }
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(Microsoft.AspNetCore.Builder.IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {
                    HotModuleReplacement = true,
                    ReactHotModuleReplacement = true
                });
            }
            else {
                app.UseExceptionHandler("/Home/Error");
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

            app.UseStaticFiles();

            app.UseIdentity();

            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715
            // https://docs.microsoft.com/en-gb/aspnet/core/security/authentication/social/index
            app.UseGoogleAuthentication(new GoogleOptions() {
                ClientId = Configuration.GetValue<string>("OAuth:Google:ClientId"),
                ClientSecret = Configuration.GetValue<string>("OAuth:Google:ClientSecret")
            });

            app.UseFacebookAuthentication(new FacebookOptions() {
                ClientId = Configuration.GetValue<string>("OAuth:Facebook:ClientId"),
                ClientSecret = Configuration.GetValue<string>("OAuth:Facebook:ClientSecret")
            });

            app.UseMicrosoftAccountAuthentication(new MicrosoftAccountOptions() {
                ClientId = Configuration.GetValue<string>("OAuth:Microsoft:ClientId"),
                ClientSecret = Configuration.GetValue<string>("OAuth:Microsoft:ClientSecret")
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

            //CreateRoles(app.ApplicationServices);
        }

        DocumentClient InitializeDocumentClient(Uri endpointUri, string authorizationKey) {
            // Create a DocumentClient and an initial collection (if it does not exist yet) for sample purposes
            var client = new DocumentClient(endpointUri, authorizationKey, new ConnectionPolicy { EnableEndpointDiscovery = false });
            try {
                // Does the DB exist?
                var db = client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DocumentDB.DatabaseId)).Result;
            }
            catch (AggregateException ae) {
                ae.Handle(ex => {
                    if (ex.GetType() == typeof(DocumentClientException) && ((DocumentClientException)ex).StatusCode == HttpStatusCode.NotFound) {
                        // Create DB
                        var db = client.CreateDatabaseAsync(new Database() { Id = DocumentDB.DatabaseId }).Result;
                        return true;
                    }

                    return false;
                });
            }

            try {
                // Does the Collection exist?
                var collection = client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DocumentDB.DatabaseId, DocumentDB.AspNetIdentity)).Result;
            }
            catch (AggregateException ae) {
                ae.Handle(ex => {
                    if (ex.GetType() == typeof(DocumentClientException) && ((DocumentClientException)ex).StatusCode == HttpStatusCode.NotFound) {
                        var collection = new DocumentCollection() { Id = DocumentDB.AspNetIdentity };
                        collection = client.CreateDocumentCollectionAsync(UriFactory.CreateDatabaseUri(DocumentDB.DatabaseId), collection).Result;

                        return true;
                    }

                    return false;
                });
            }

            try {
                // Does the Collection exist?
                var collection = client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DocumentDB.DatabaseId, DocumentDB.AspNetIdentityRoles)).Result;
            }
            catch (AggregateException ae) {
                ae.Handle(ex => {
                    if (ex.GetType() == typeof(DocumentClientException) && ((DocumentClientException)ex).StatusCode == HttpStatusCode.NotFound) {
                        var collection = new DocumentCollection() { Id = DocumentDB.AspNetIdentityRoles };
                        collection = client.CreateDocumentCollectionAsync(UriFactory.CreateDatabaseUri(DocumentDB.DatabaseId), collection).Result;

                        return true;
                    }

                    return false;
                });
            }

            return client;
        }

        //void CreateRoles(IServiceProvider serviceProvider) {
        //    //initializing custom roles 
        //    var RoleManager = serviceProvider.GetRequiredService<RoleManager<DocumentDbIdentityRole>>();
        //    var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        //    string[] roleNames = { "SysAdmin", "TeaManager", "Collector", "Member" };
        //    IdentityResult roleResult;

        //    foreach (var roleName in roleNames) {
        //        var roleExist = RoleManager.RoleExistsAsync(roleName).Result;
        //        if (!roleExist) {
        //            //create the roles and seed them to the database: Question 1
        //            roleResult = RoleManager.CreateAsync(new DocumentDbIdentityRole { Name = roleName }).Result;
        //        }
        //    }

        //    //Here you could create a super user who will maintain the web app
        //    var poweruser = new ApplicationUser {

        //        UserName = "Aleksander Spro",
        //        Email = "abspro@gmail.com",
        //    };
        //    //Ensure you have these values in your appsettings.json file
        //    var userPWD = "dummy";
        //    var _user = UserManager.FindByEmailAsync("gledesrus@hotmail.com").Result;

        //    if (_user == null) {
        //        var createPowerUser = UserManager.CreateAsync(poweruser, userPWD).Result;
        //        if (createPowerUser.Succeeded) {
        //            //here we tie the new user to the role
        //            roleResult = UserManager.AddToRoleAsync(poweruser, "SysAdmin").Result;

        //        }
        //    }
        //    else {
        //        roleResult = UserManager.AddToRoleAsync(_user, "SysAdmin").Result;
        //    }
        //}

        //async Task CreateRoles(IServiceProvider serviceProvider) {
        //    //initializing custom roles 
        //    var RoleManager = serviceProvider.GetRequiredService<RoleManager<DocumentDbIdentityRole>>();
        //    var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        //    string[] roleNames = { "SysAdmin", "TeaManager", "Collector", "Member" };
        //    IdentityResult roleResult;

        //    foreach (var roleName in roleNames) {
        //        var roleExist = await RoleManager.RoleExistsAsync(roleName);
        //        if (!roleExist) {
        //            //create the roles and seed them to the database: Question 1
        //            roleResult = await RoleManager.CreateAsync(new DocumentDbIdentityRole { Name = roleName });
        //        }
        //    }

        //    //Here you could create a super user who will maintain the web app
        //    var poweruser = new ApplicationUser {

        //        UserName = Configuration["AppSettings:UserName"],
        //        Email = Configuration["AppSettings:UserEmail"],
        //    };
        //    //Ensure you have these values in your appsettings.json file
        //    var userPWD = Configuration["AppSettings:UserPassword"];
        //    var _user = await UserManager.FindByEmailAsync(Configuration["AppSettings:AdminUserEmail"]);

        //    if (_user == null) {
        //        var createPowerUser = await UserManager.CreateAsync(poweruser, userPWD);
        //        if (createPowerUser.Succeeded) {
        //            //here we tie the new user to the role
        //            await UserManager.AddToRoleAsync(poweruser, "Admin");

        //        }
        //    }
        //}
    }
}
