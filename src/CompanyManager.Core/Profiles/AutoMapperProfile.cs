using AutoMapper;
using CompanyManager.Core.Models;
using CompanyManager.Shared;

namespace CompanyManager.Core.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Customer, EditCustomerModel>().ReverseMap();
            CreateMap<Offer, DisplayOfferModel>();
        }
    }
}
