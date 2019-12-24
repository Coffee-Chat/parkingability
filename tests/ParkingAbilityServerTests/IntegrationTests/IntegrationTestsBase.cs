using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParkingAbilityServerTests.IntegrationTests
{
    public abstract class IntegrationTestsBase
    {
        protected static async Task<HttpResponseMessage> SendJsonAsync<T>(HttpMethod method, T model, string path)
        {
            using HttpRequestMessage requestMessage = new HttpRequestMessage(method, path);
            string body = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(body, Encoding.UTF8, "application/json");
            requestMessage.Content = content;
            requestMessage.RequestUri = new Uri(path, UriKind.Relative);
            return await TestAssemblyEvents.HttpClient.SendAsync(requestMessage);
        }
    }
}
