using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkingAbilityServer;
using ParkingAbilityServer.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace ParkingAbilityServerTests
{
    [TestClass]
    public static class TestAssemblyEvents
    {
        private static TestServer server;
        public static HttpClient HttpClient { get; private set; }

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureTestServices(services =>
                {
                    services.AddTransient<ILocationsRepository, MemoryLocationsRepository>();
                }));
            HttpClient = server.CreateClient();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            server?.Dispose();
        }
    }
}
