using AutoMapper;
using CompanyManager.Server.Repositories;
using CompanyManager.Shared;

namespace CompanyManager.Server.Services
{
    public interface IOfferService
    {
        Task<IEnumerable<OffersGroup>> GetAllOffersByParentCategory();
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

        public async Task<IEnumerable<OffersGroup>> GetAllOffersByParentCategory()
        {
            var offers = await _offerRepository.GetAllOffers();
            var offersGrouped = offers.GroupBy(o => o.OfferCategory.Name).Select(grp => grp);
            var offersViewModel = new List<OffersGroup>();

            foreach (var group in offersGrouped)
            {
                var offersInGroup = group.AsEnumerable();

                offersViewModel.Add(new OffersGroup
                {
                    OfferGroupName = group.Key,
                    Offers = _mapper.Map<IEnumerable<OfferViewModel>>(offersInGroup)
                });
            }

            return offersViewModel;
        }
    }
}
