using CompanyManager.Server.Services;
using CompanyManager.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManager.Server.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointment(int? appointmentId)
        {
            var appointment = await _appointmentService.GetAppointment(appointmentId);
            return Ok(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> GetAppointmentsInRange([FromBody] AppointmentsRange appointmentsRange)
        {
            var appointments = await _appointmentService.GetAppointmentsInRange(appointmentsRange);
            return Ok(appointments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment([FromBody] EditAppointmentModel appointment)
        {
            if (appointment == null || ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            
            if(await _appointmentService.CheckForConflicts(appointment))
            {
                ModelState.AddModelError("DateConflict", "Czas trwania wizyty pokrywa się z czasem innej wizyty.");
                return BadRequest(ModelState);
            }

            var result = await _appointmentService.CreateAppointment(appointment);

            return result ? Ok(ModelState) : BadRequest(ModelState);
        }
    }
}
