using CompanyManager.Core.Services;
using CompanyManager.Shared;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManager.Api.Controllers
{
    [Route("api/offer")]
    public class OfferController : ApiControllerBase
    {
        private readonly IOfferService _offerService;

        public OfferController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(List<OffersGroup>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOffers([FromBody] OffersRequest offersRequest)
        {
            var offers = await _offerService.GetAllOffersByParentCategory(offersRequest.SelectedOffers);

            return Ok(offers);
        }
    }
}
