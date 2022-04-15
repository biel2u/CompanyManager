using CompanyManager.Core.Validators;
using CompanyManager.Core.Services;
using CompanyManager.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CompanyManager.Api.Controllers
{
    [Route("api/appointment")]
    public class AppointmentController : ApiControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IAppointmentsOffersService _appointmentsOffersService;
        private readonly IAppointmentValidator _appointmentValidator;

        public AppointmentController(IAppointmentService appointmentService, IAppointmentValidator appointmentValidator, IAppointmentsOffersService appointmentsOffersService)
        {
            _appointmentService = appointmentService;
            _appointmentValidator = appointmentValidator;
            _appointmentsOffersService = appointmentsOffersService;
        }

        [HttpGet("{id?}")]
        [ProducesResponseType(typeof(EditAppointmentModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int? id)
        {
            var appointment = await _appointmentService.GetAppointment(id);
            if (appointment == null) return NotFound();

            return Ok(appointment);
        }

        [HttpPost("range")]
        [ProducesResponseType(typeof(List<DisplayAppointmentModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetInRange([FromBody] AppointmentsRange appointmentsRange)
        {
            var appointments = await _appointmentService.GetAppointmentsInRange(appointmentsRange);

            return Ok(appointments);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] EditAppointmentModel appointment)
        {
            await _appointmentValidator.SetModelStateErrors(appointment, ModelState);
            if (appointment == null || ModelState.IsValid == false || ModelState.ErrorCount > 0)
            {
                return BadRequest(ModelState);
            }

            var result = await _appointmentsOffersService.CreateAppointmentWithOffers(appointment);
            return result ? Created("appointment", ModelState) : BadRequest(ModelState);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ModelStateDictionary), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] EditAppointmentModel appointment)
        {
            await _appointmentValidator.SetModelStateErrors(appointment, ModelState);
            if (appointment == null || ModelState.IsValid == false || ModelState.ErrorCount > 0)
            {
                return BadRequest(ModelState);
            }

            var result = await _appointmentService.UpdateAppointment(appointment);
            return result ? Ok(ModelState) : BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var appointmentDeleted = await _appointmentService.DeleteAppointment(id);
            if (appointmentDeleted == false) return NotFound();

            return NoContent();
        }
    }
}
