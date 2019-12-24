using Microsoft.VisualStudio.TestTools.UnitTesting;
using ParkingAbilityServer.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ParkingAbilityServerTests.IntegrationTests
{
    [TestClass]
    public class LocationsApiTests : IntegrationTestsBase
    {
        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestMethod, TestCategory("L2")]
        [DataRow("api/locales/WA/locations", 89.0F, 179.0F, HttpStatusCode.Created)]
        [DataRow("api/locales/WA/locations", 90.0F, 180.0F, HttpStatusCode.Created)]
        [DataRow("api/locales/WA/locations", -90.0F, -180.0F, HttpStatusCode.Created)]
        [DataRow("api/locales/WA/locations", 91.0F, 181.0F, HttpStatusCode.BadRequest)]
        [DataRow("api/locales/WA/locations", 89.0F, 181.0F, HttpStatusCode.BadRequest)]
        [DataRow("api/locales/WA/locations", 91.0F, 179.0F, HttpStatusCode.BadRequest)]
        [DataRow("api/locales/WA/locations", -91.0F, -179.0F, HttpStatusCode.BadRequest)]
        [DataRow("api/locales/WA/locations", -90.0F, -181.0F, HttpStatusCode.BadRequest)]
        public async Task PostLocationsAsync(string path, float lat, float lon, HttpStatusCode expectedResult)
        {
            LocationRequest locationRequest = new LocationRequest()
            {
                Latitude = lat,
                Longitude = lon,
                Timestamp = (long)(DateTime.UtcNow - DateTime.UnixEpoch).TotalMilliseconds
            };
            HttpResponseMessage responseMessage = await SendJsonAsync(HttpMethod.Post, locationRequest, path);
            Assert.AreEqual(expectedResult, responseMessage.StatusCode);
        }
    }
}
