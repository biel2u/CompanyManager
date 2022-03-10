using CompanyManager.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManager.Server.Features.Appointments
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        //[HttpPost]
        //public  Create(AppointmentViewModel appointment)
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateTime.Now.AddDays(index),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}
        public AppointmentController()
        {

        }

        [HttpGet]
        public AppointmentViewModel Get()
        {
            return new AppointmentViewModel
            {
                //Customer = new CustomerViewModel(),
                Date = DateTime.Now,
                Note = "lol notka",
                Offer = new OfferViewModel(),
                Status = AppointmentStatus.Pending
            };
        }
    }
}
