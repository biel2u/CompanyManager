using AutoMapper;
using CompanyManager.Core.Repositories;
using CompanyManager.Shared;
using Microsoft.EntityFrameworkCore;

namespace CompanyManager.Core.Services
{
    public interface IOfferService
    {
        Task<List<OffersGroup>> GetAllOffersByParentCategory(List<DisplayOfferModel> selectedOffers);
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

        public async Task<List<OffersGroup>> GetAllOffersByParentCategory(List<DisplayOfferModel> selectedOffers)
        {
            var offersByCategory = await GroupOffersByCategory();
            if(selectedOffers != null && selectedOffers.Any())
            {
                UpdateSelectedOffers(offersByCategory, selectedOffers);
            }

            return offersByCategory;          
        }

        private async Task<List<OffersGroup>> GroupOffersByCategory()
        {
            var offers = await _offerRepository.GetAllOffers().ToListAsync();

            var offersGrouped = offers.GroupBy(o => o.OfferCategory.Name).Select(grp => grp);
            var offersByCategory = new List<OffersGroup>();

            foreach (var group in offersGrouped)
            {
                var offersInGroup = group.AsEnumerable();

                offersByCategory.Add(new OffersGroup
                {
                    OfferGroupName = group.Key,
                    Offers = _mapper.Map<IEnumerable<DisplayOfferModel>>(offersInGroup)
                });
            }

            return offersByCategory;
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
