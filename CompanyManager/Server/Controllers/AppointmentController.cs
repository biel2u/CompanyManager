using CompanyManager.Server.Services;
using CompanyManager.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManager.Server.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int? appointmentId)
        {
            var appointment = await _appointmentService.GetAppointment(appointmentId);
            return Ok(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AppointmentEditForm appointment)
        {
            if (appointment == null || ModelState.IsValid == false)
            {
                return BadRequest();
            }
            
            if(await _appointmentService.CheckForConflicts(appointment))
            {
                ModelState.AddModelError("DateConflict", "Czas trwania wizyty pokrywa się z czasem innej wizyty.");
                return BadRequest();
            }

            var result = await _appointmentService.CreateAppointment(appointment);

            return result ? Ok() : BadRequest();
        }
    }
}
