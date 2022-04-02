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
        public async Task<IActionResult> GetAppointment(int? id)
        {
            var appointment = await _appointmentService.GetAppointment(id);
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
            
            var errors = await _appointmentService.ValidateAppointment(appointment);

            foreach(var error in errors)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }

            if(errors.Any())
            {
                return BadRequest(ModelState);
            }

            var result = await _appointmentService.CreateAppointment(appointment);

            return result ? Ok(ModelState) : BadRequest(ModelState);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointmentDeleted = await _appointmentService.DeleteAppointment(id);
            if (appointmentDeleted == false) return NotFound();

            return NoContent();
        }
    }
}
