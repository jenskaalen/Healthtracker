using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Healthtracker.Web.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Google;
using Healthtracker.Web.Repositories;
using System.Net.Http;
using Healthtracker.Web.Services;
using Healthtracker.Web.Model;
using Healthtracker.Web.Hubs;
using Healthtracker.Web.Services.Synchronization;
using Microsoft.AspNetCore.SignalR;
using Raven.Client.Documents;
using System.Security.Cryptography.X509Certificates;

namespace Healthtracker.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            //services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, SynchronizationService >();
            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, SynchronizationService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.Configure<IntegrationConfig>(Configuration);

            var config = Configuration.Get<IntegrationConfig>();

            services.AddAuthentication().AddGoogle(options => {
                options.ClientId = config.GoogleClientId;
                options.ClientSecret = config.GoogleClientSecret;
                options.CallbackPath = "/signin-google";
                options.SaveTokens = true;
            }
            );

            services.AddSingleton<IUserIdProvider, UsernameIdProvider>();
            //TODO: lazy load this
            services.AddSingleton<IDocumentStore>(GetDocumentStore());
            services.AddSingleton<IActivitySuggestionsService, ActivitySuggestionsService>();
            services.AddSingleton<ISupplementSuggestionsService, SupplementSuggestionsService>();
            services.AddSingleton<ISyncQueue, SyncQueue>();
            services.AddSingleton(typeof(ILogRepository), typeof(RavenDbRepository));
            services.AddSingleton(typeof(IFitbitRepository), typeof(FitbitRepository));
            services.AddSingleton(typeof(FitbitTokenStorage));
            services.AddHttpClient();
            services.AddMemoryCache();
            //services.AddHostedService<SynchronizationService>();
            services.AddSignalR();

            
        }

        private static DocumentStore GetDocumentStore()
        {
            X509Certificate2 clientCertificate = new X509Certificate2("certi.pfx", "FEANturi2");

            var doc = new DocumentStore()
            {
                Urls = new string[] { "https://a.healthbonto.ravendb.community:4343" },
                Certificate = clientCertificate,
                Database = "LogDb",
                Conventions =
                    {
                        FindIdentityProperty = prop => prop.Name == "DocumentId"
                    }
            };

            doc.Initialize();
            return doc; 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json",
                             optional: false,
                             reloadOnChange: true)
                .AddEnvironmentVariables();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

                builder.AddUserSecrets<Startup>();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseSignalR(r =>
            {
                r.MapHub<NotificationHub>("/notificationHub");
            });

            app.UseMvc();
        }
    }
}
