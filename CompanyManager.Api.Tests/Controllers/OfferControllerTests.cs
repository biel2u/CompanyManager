using AutoFixture.Xunit2;
using CompanyManager.Api.Controllers;
using CompanyManager.Core.Services;
using CompanyManager.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CompanyManager.Api.Tests.Controllers
{
    public class OfferControllerTests : ControllerTestsBase<OfferController>
    {
        private readonly Mock<IOfferService> _offerService;

        public OfferControllerTests()
        {
            _offerService = Mocker.GetMock<IOfferService>();
        }

        [Fact]
        public async Task GetOffers_ShouldCallGetAllOffersByParentCategory()
        {
            //given
            var offersRequest = new OffersRequest { SelectedOffers = new List<DisplayOfferModel>() };
            //when
            await Controller.GetOffers(offersRequest);

            //then
            _offerService.Verify(x => x.GetAllOffersByParentCategory(offersRequest.SelectedOffers), Times.Once);
        }

        [Theory, AutoData]
        public async Task GetOffers_ShouldReturnOkStatusWithExpectedResult(List<OffersGroup> offersGroups)
        {
            //given
            _offerService.Setup(x => x.GetAllOffersByParentCategory(It.IsAny<List<DisplayOfferModel>>()))
                .ReturnsAsync(offersGroups);

            //when
            var result = await Controller.GetOffers(new OffersRequest()) as OkObjectResult;

            //then
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeAssignableTo<List<OffersGroup>>()
                .And.BeEquivalentTo(offersGroups);           
        }
    }
}
