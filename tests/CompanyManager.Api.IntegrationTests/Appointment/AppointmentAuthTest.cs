using CompanyManager.Api.IntegrationTests.Extensions;
using CompanyManager.Api.IntegrationTests.Infrastructure;
using CompanyManager.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace CompanyManager.Api.IntegrationTests.Appointment
{
    public class AppointmentAuthTest : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private const string BaseUrl = "api/appointment";

        public AppointmentAuthTest(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions());
        }

        [Fact]
        public async Task Get_ShouldReturnUnauthorized_WhenUserUnauthenticated()
        {
            var result = await _client.GetAsync($"{BaseUrl}/1");

            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetInRange_ShouldReturnUnauthorized_WhenUserUnauthenticated()
        {
            var request = new AppointmentsRange();
            var result = await _client.PostAsync($"{BaseUrl}/range", request.ToStringContent());

            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Create_ShouldReturnUnauthorized_WhenUserUnauthenticated()
        {
            var request = new EditAppointmentModel();
            var result = await _client.PostAsync(BaseUrl, request.ToStringContent());

            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Update_ShouldReturnUnauthorized_WhenUserUnauthenticated()
        {
            var request = new EditAppointmentModel();
            var result = await _client.PutAsync(BaseUrl, request.ToStringContent());

            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Delete_ShouldReturnUnauthorized_WhenUserUnauthenticated()
        {
            var result = await _client.DeleteAsync($"{BaseUrl}/1");

            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}
