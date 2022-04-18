using CompanyManager.Api.IntegrationTests.Extensions;
using CompanyManager.Api.IntegrationTests.Infrastructure;
using CompanyManager.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace CompanyManager.Api.IntegrationTests.Customer
{
    public class CustomerAuthTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private const string BaseUrl = "api/customer";

        public CustomerAuthTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions());
        }

        [Fact]
        public async Task GetByValue_ShouldReturnUnauthorized_WhenUserUnauthenticated()
        {
            var result = await _client.GetAsync($"{BaseUrl}/value");

            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Create_ShouldReturnUnauthorized_WhenUserUnauthenticated()
        {
            var request = new EditCustomerModel();
            var result = await _client.PostAsync(BaseUrl, request.ToStringContent());

            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
