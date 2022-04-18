using CompanyManager.Api.IntegrationTests.Extensions;
using CompanyManager.Api.IntegrationTests.Infrastructure;
using CompanyManager.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CompanyManager.Api.IntegrationTests
{
    public class CustomerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private const string BaseUrl = "api/customer";

        public CustomerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                            "Test", options => { });
                });
            }).CreateClient();
        }

        [Fact]
        public async Task GetByValue_ShouldReturnOkWithExpectedResult_WhenSearchValueFound()
        {
            var result = await _client.GetAsync($"{BaseUrl}/Fro");

            result.EnsureSuccessStatusCode();
            var customer = await result.Content.ReadAsJsonAsync<List<string>>();
            customer.Should().HaveCount(1);
            customer.Contains("Baggins Frodo (123456789)").Should().BeTrue();
        }

        [Fact]
        public async Task GetByValue_ShouldReturnOkWithEmptyCollection_WhenSearchValueNotFound()
        {
            var result = await _client.GetAsync($"{BaseUrl}/Sam");

            result.EnsureSuccessStatusCode();
            var customer = await result.Content.ReadAsJsonAsync<List<string>>();
            customer.Should().HaveCount(0);
        }

        [Fact]
        public async Task Create_ShouldReturnCreated_WhenSuccessfullyCreated()
        {
            var request = new EditCustomerModel 
            {
                Name = "Samwise",
                Surname = "Gamgee",
                Phone = "987654321"
            };

            var result = await _client.PostAsync(BaseUrl, request.ToStringContent());

            result.EnsureSuccessStatusCode();
        }
    }
}
