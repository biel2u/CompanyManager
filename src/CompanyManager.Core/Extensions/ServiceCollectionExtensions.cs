using CompanyManager.Core.Profiles;
using CompanyManager.Core.Repositories;
using CompanyManager.Core.Services;
using CompanyManager.Core.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace CompanyManager.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services
                .AddAutoMapper(config => { config.AddProfile(new AutoMapperProfile()); })
                .AddTransient<ICustomerRepository, CustomerRepository>()
                .AddTransient<IOfferRepository, OfferRepository>()
                .AddTransient<ICustomerService, CustomerService>()
                .AddTransient<IOfferService, OfferService>()
                .AddTransient<IAppointmentService, AppointmentService>()
                .AddTransient<IDateTimeProvider, DateTimeProvider>()
                .AddTransient<IAppointmentRepository, AppointmentRepository>()
                .AddTransient<IAppointmentOfferRepository, AppointmentOfferRepository>()
                .AddTransient<IAppointmentValidator, AppointmentValidator>()
                .AddTransient<ICustomerValidator, CustomerValidator>()
                .AddTransient<IAppointmentsOffersService, AppointmentsOffersService>();

            return services;
        }
    }
}
