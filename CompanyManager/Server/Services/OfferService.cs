using AutoMapper;
using CompanyManager.Server.Repositories;
using CompanyManager.Shared;

namespace CompanyManager.Server.Services
{
    public interface IOfferService
    {
        Task<IEnumerable<IGrouping<string, OfferViewModel>>> GetAllOffersByParentCategory();
    }

    public class OfferService : IOfferService
    {
        private readonly IOfferRepository _offerRepository;
        private readonly IMapper _mapper;

        public OfferService(IOfferRepository offerRepository, IMapper mapper)
        {
            _offerRepository = offerRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<IGrouping<string, OfferViewModel>>> GetAllOffersByParentCategory()
        {
            var offers = await _offerRepository.GetAllOffers();
            var offersViewModel = _mapper.Map<List<OfferViewModel>>(offers);
            var offersGrouped = offersViewModel.GroupBy(o => o.OfferCategoryName).Select(grp => grp);

            return offersGrouped;
        }
    }
}
