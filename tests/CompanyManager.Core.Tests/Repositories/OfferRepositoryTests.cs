using AutoFixture;
using CompanyManager.Core.Data;
using CompanyManager.Core.Models;
using CompanyManager.Core.Repositories;
using Duende.IdentityServer.EntityFramework.Options;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace CompanyManager.Core.Tests.Repositories
{
    public class OfferRepositoryTests
    {
        private static readonly Fixture _fixture = new Fixture();

        private readonly OfferRepository _repository;
        private readonly Mock<ApplicationDbContext> _dbContextMock;

        public OfferRepositoryTests()
        {
            _dbContextMock = new Mock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options, It.IsAny<IOptions<OperationalStoreOptions>>());
            _repository = new OfferRepository(_dbContextMock.Object);
        }

        [Fact]
        public async Task GetAllOffers_ShouldReturnExpectedOffersCount()
        {
            //given
            var offers = _fixture.Build<Offer>()
                .Without(x => x.AppointmentOffers)
                .Without(x => x.OfferCategory)
                .CreateMany(10);

            var x = _dbContextMock.Setup(x => x.Offers).Returns(offers.GetMockDbSetObject());

            //when
            var result = await _repository.GetAllOffers().ToListAsync();

            //then
            result.Should().HaveCount(10);
        }
    }
}
