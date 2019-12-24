using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ParkingAbilityServer.BusinessLayer;
using ParkingAbilityServer.Models;

namespace ParkingAbilityServer
{
    public class Startup
    {
        private static readonly AzureServiceTokenProvider serviceTokenProvider = 
            new AzureServiceTokenProvider(connectionString: "RunAs=App;AppId=12f6dcda-dc91-4045-aebd-36c278a3d97a");

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "We need to hold on to the instance for renewal.")]
        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Required by AspNetCore")]
        [SuppressMessage("Design", "ASP0000:Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'", Justification = "We need config logic for dependecies")]
        public void ConfigureServices(IServiceCollection services)
        {
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            IWebHostEnvironment env = serviceProvider.GetService<IWebHostEnvironment>();
            if (env.IsDevelopment())
            {
                Environment.SetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY", "32c9be7e-b57f-41d3-88ab-e3b282a050dd");
                services.AddSingleton(CloudStorageAccount.DevelopmentStorageAccount);
            }
            else
            {
                NewTokenAndFrequency tokenAndFrequency = TokenRenewer(this, CancellationToken.None).GetAwaiter().GetResult();
                TokenCredential tokenCredential = new TokenCredential(
                    initialToken: tokenAndFrequency.Token,
                    periodicTokenRenewer: TokenRenewer,
                    state: null,
                    renewFrequency: tokenAndFrequency.Frequency.Value);

                services.AddSingleton(new CloudStorageAccount(new StorageCredentials(tokenCredential), "parkingability", "core.windows.net", true));
            }

            services.AddSingleton<ILocationsRepository, StorageLocationsRepository>();
            services.AddSingleton<IRepository, MemoryRepository>();
            services.AddSingleton<IFlightProvider, MemoryFlightProvider>();
            services.AddControllersWithViews(options => 
            {
                options.ModelBinderProviders.Insert(0, new HeadersModelBinderProvider());
                options.RespectBrowserAcceptHeader = true;
            });
        }

        [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Required by AspNetCore")]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private async Task<NewTokenAndFrequency> TokenRenewer(object state, CancellationToken cancellationToken)
        {
            AppAuthenticationResult authResult = await serviceTokenProvider.GetAuthenticationResultAsync("https://storage.azure.com/", cancellationToken: cancellationToken);

            // Schedule the next frequency 5 minutes before the new token expires.
            TimeSpan next = authResult.ExpiresOn - DateTimeOffset.UtcNow - TimeSpan.FromMinutes(5);
            if (next.Ticks < 0)
            {
                next = default(TimeSpan);
            }

            return new NewTokenAndFrequency(authResult.AccessToken, next);
        }
    }
}
