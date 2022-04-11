using CompanyManager.Server.Services;
using CompanyManager.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManager.Server.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private readonly IOfferService _offerService;

        public OfferController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<OffersGroup>>> GetOffers(IEnumerable<DisplayOfferModel>? selectedOffers)
        {
            var offers = await _offerService.GetAllOffersByParentCategory(selectedOffers);
            return Ok(offers);
        }
    }
}
