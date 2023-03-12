using System.Text;
using JsonNet.PrivateSettersContractResolvers;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Wati.Template.Api;

namespace Wati.Template.Service.IntegrationTests.Endpoints
{
    public class BaseEndpointTests : IClassFixture<CustomWebAppFactory<Program>>
    {
        private readonly CustomWebAppFactory<Program> _factory;

        public BaseEndpointTests(CustomWebAppFactory<Program> factory)
        {
            _factory = factory;
        }

        protected HttpClient CreateClient(WebApplicationFactoryClientOptions options) => _factory.CreateClient(options);

        protected HttpClient CreateClient() => _factory.CreateClient(new WebApplicationFactoryClientOptions { BaseAddress = new Uri(Constant.BaseUrl) });

        protected StringContent ParseJsonToString<T>(T model) =>
            new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

        protected async ValueTask<T> ParseResponse<T>(HttpResponseMessage response)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        protected T ParseEndpointData<T>(string filePath)
        {
            var data = System.IO.File.ReadAllText($@"Data/{filePath}");
            var settings = new JsonSerializerSettings { ContractResolver = new PrivateSetterContractResolver() };
            return JsonConvert.DeserializeObject<T>(data, settings);
        }
    }
}
