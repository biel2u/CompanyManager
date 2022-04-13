using CompanyManager.Server.Validators;
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
        private IAppointmentsOffersService _appointmentsOffersService;
        private IAppointmentValidator _appointmentValidator;

        public AppointmentController(IAppointmentService appointmentService, IAppointmentValidator appointmentValidator, IAppointmentsOffersService appointmentsOffersService)
        {
            _appointmentService = appointmentService;
            _appointmentValidator = appointmentValidator;
            _appointmentsOffersService = appointmentsOffersService;
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
            await _appointmentValidator.SetModelStateErrors(appointment, ModelState);
            if (appointment == null || ModelState.IsValid == false || ModelState.ErrorCount > 0)
            {
                return BadRequest(ModelState);
            }

            var result = await _appointmentsOffersService.CreateAppointmentWithOffers(appointment);
            return result ? Ok(ModelState) : BadRequest(ModelState);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateAppointment([FromBody] EditAppointmentModel appointment)
        {
            await _appointmentValidator.SetModelStateErrors(appointment, ModelState);
            if (appointment == null || ModelState.IsValid == false || ModelState.ErrorCount > 0)
            {
                return BadRequest(ModelState);
            }

            var result = await _appointmentService.UpdateAppointment(appointment);
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
