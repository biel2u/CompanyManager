using CompanyManager.Core.Services;
using CompanyManager.Shared;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManager.Api.Controllers
{
    [Route("api/offer")]
    public class OfferController : ControllerBase
    {
        private readonly IOfferService _offerService;

        public OfferController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(List<OffersGroup>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<OffersGroup>>> GetOffers([FromBody] OffersRequest offersRequest)
        {
            var offers = await _offerService.GetAllOffersByParentCategory(offersRequest.SelectedOffers);
            if(offers.Any() == false) return NotFound();

            return Ok(offers);
        }
    }
}
