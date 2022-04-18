using CompanyManager.Api.IntegrationTests.Extensions;
using CompanyManager.Api.IntegrationTests.Infrastructure;
using CompanyManager.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace CompanyManager.Api.IntegrationTests.Offer
{
    public class OfferAuthTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public OfferAuthTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions());
        }

        [Fact]
        public async Task GetOffers_ShouldReturnUnauthorized_WhenUserUnauthenticated()
        {
            var request = new OffersRequest();
            var result = await _client.PostAsync("api/offer", request.ToStringContent());

            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
