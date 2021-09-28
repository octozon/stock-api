using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using Stocks.Application.Queries;
using Stocks.Application.Queries.Stocks;

namespace Stocks.Api.Tests
{
    public class StocksControllerTests : StocksControllerTestsFixture
    {
        [Test]
        public async Task Can_get_stocks()
        {
            /*// Act
            HttpResponseMessage actual = await HttpClient.GetAsync("/stocks?page=1&size=30");

            // Assert
            actual.StatusCode.Should().Be(HttpStatusCode.OK);*/
        }
    }

    internal static class HttpExtensions
    {
        public static async Task<T> Result<T>(this HttpResponseMessage response) where T : class
        {
            string content = await GetContentAsync(response);

            return JsonConvert.DeserializeObject<T>(content);
        }

        private static async Task<string> GetContentAsync(this HttpResponseMessage response)
        {
            string content = await response.Content.ReadAsStringAsync();

            return content;
        }
    }

    [TestFixture]
    public abstract class StocksControllerTestsFixture : WebApplicationFactory<Startup>
    {
        [SetUp]
        public void SetUp()
        {
            HttpClient = base.CreateClient();
        }

        public HttpClient HttpClient { get; set; }
    }
}