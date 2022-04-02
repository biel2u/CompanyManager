using AutoMapper;
using CompanyManager.Server.Repositories;
using CompanyManager.Shared;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Server.Services
{
    public interface IOfferService
    {
        Task<IEnumerable<OffersGroup>> GetAllOffersByParentCategory(IEnumerable<DisplayOfferModel>? selectedOffers);
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

        public async Task<IEnumerable<OffersGroup>> GetAllOffersByParentCategory(IEnumerable<DisplayOfferModel>? selectedOffers)
        {
            var offers = await _offerRepository.GetAllOffers().ToListAsync();          
            var offersGrouped = offers.GroupBy(o => o.OfferCategory.Name).Select(grp => grp);
            var offersViewModel = new List<OffersGroup>();

            foreach (var group in offersGrouped)
            {
                var offersInGroup = group.AsEnumerable();

                offersViewModel.Add(new OffersGroup
                {
                    OfferGroupName = group.Key,
                    Offers = _mapper.Map<IEnumerable<DisplayOfferModel>>(offersInGroup)
                });
            }

            if(selectedOffers != null && selectedOffers.Any())
            {
                UpdateSelectedOffers(offersViewModel, selectedOffers);
            }

            return offersViewModel;          
        }

        private void UpdateSelectedOffers(IEnumerable<OffersGroup> offersGroups, IEnumerable<DisplayOfferModel> selectedOffers)
        {
            foreach (var group in offersGroups)
            {
                foreach (var offer in group.Offers)
                {
                    if (selectedOffers.Any(s => s.Id == offer.Id))
                    {
                        offer.IsSelected = true;
                    }
                }
            }
        }
    }
}
