using CompanyManager.Api.IntegrationTests.Extensions;
using CompanyManager.Api.IntegrationTests.Infrastructure;
using CompanyManager.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
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
        public async Task Post_ShouldReturnUnauthorizedForUnauthenticatedUser()
        {
            var request = new OffersRequest();
            var result = await _client.PostAsync("api/offer", request.ToStringContent());

            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
