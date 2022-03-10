using AutoMapper;
using CompanyManager.Server.Models;
using CompanyManager.Shared;

namespace CompanyManager.Server.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Customer, CustomerViewModel>().ReverseMap();
        }
    }
}
