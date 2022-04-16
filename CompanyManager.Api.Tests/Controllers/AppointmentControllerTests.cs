using AutoFixture.Xunit2;
using CompanyManager.Api.Controllers;
using CompanyManager.Core.Services;
using CompanyManager.Core.Validators;
using CompanyManager.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CompanyManager.Api.Tests.Controllers
{
    public class AppointmentControllerTests : ControllerTestsBase<AppointmentController>
    {
        private readonly Mock<IAppointmentService> _appointmentService;
        private readonly Mock<IAppointmentsOffersService> _appointmentsOffersService;
        private readonly Mock<IAppointmentValidator> _appointmentValidator;

        public AppointmentControllerTests()
        {
            _appointmentService = Mocker.GetMock<IAppointmentService>();
            _appointmentsOffersService = Mocker.GetMock<IAppointmentsOffersService>();
            _appointmentValidator = Mocker.GetMock<IAppointmentValidator>();
        }

        [Fact]
        public async Task GetInRange_ShouldCallGetAppointmentsInRanged()
        {
            //given
            var appointmentsRange = new AppointmentsRange
            {
                StartDate = new DateTime(2022, 5, 4),
                EndDate = new DateTime(2022, 5, 11)
            };

            //when
            await Controller.GetInRange(appointmentsRange);

            //then
            _appointmentService.Verify(x => x.GetAppointmentsInRange(appointmentsRange), Times.Once);
        }

        [Theory, AutoData]
        public async Task GetInRange_ShouldReturnOkStatusWithExpectedResult(List<DisplayAppointmentModel> appointments)
        {
            //given
            _appointmentService.Setup(x => x.GetAppointmentsInRange(It.IsAny<AppointmentsRange>()))
                .ReturnsAsync(appointments)
                .Verifiable();

            //when
            var result = await Controller.GetInRange(new AppointmentsRange()) as OkObjectResult;

            //then
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeAssignableTo<List<DisplayAppointmentModel>>()
                .And.BeEquivalentTo(appointments);
            var value = result.Value as List<DisplayAppointmentModel>;
            value.Should().HaveCount(appointments.Count());
            _appointmentService.Verify();
        }

        [Fact]
        public async Task Get_ShouldCallGetAppointment()
        {
            //given
            const int appointmentId = 11;

            //when
            await Controller.Get(appointmentId);

            //then
            _appointmentService.Verify(x => x.GetAppointment(appointmentId), Times.Once);
        }

        [Fact]
        public async Task Get_ShouldReturnNotFoundStatus_WhenAppointmetNotFound()
        {
            //given
            _appointmentService.Setup(x => x.GetAppointment(It.IsAny<int>()))
                .ReturnsAsync(() => null);

            //when
            var result = await Controller.Get(1) as StatusCodeResult;

            //then
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Theory, AutoData]
        public async Task Get_ShouldReturnOkStatusWithExpectedResult(EditAppointmentModel appointment)
        {
            //given
            _appointmentService.Setup(x => x.GetAppointment(It.IsAny<int>()))
                .ReturnsAsync(appointment);

            //when
            var result = await Controller.Get(appointment.Id) as OkObjectResult;

            //then
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeAssignableTo<EditAppointmentModel>()
                .And.BeEquivalentTo(appointment);
        }

        [Theory, AutoData]
        public async Task Create_ShouldReturnCreatedStatusWithExpectedResult(EditAppointmentModel appointment)
        {
            //given
            _appointmentsOffersService.Setup(x => x.CreateAppointmentWithOffers(It.IsAny<EditAppointmentModel>()))
                .Verifiable();

            //when
            var result = await Controller.Create(appointment) as ObjectResult;

            //then
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
            result.Value.Should().BeAssignableTo<ModelStateDictionary>();
            _appointmentsOffersService.Verify();
        }    

        [Fact]
        public async Task Create_ShouldReturnBadRequestStatus_WhenAppointmentIsNull()
        {
            //given

            //when
            var result = await Controller.Create(null) as ObjectResult;

            //then
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Theory, AutoData]
        public async Task Create_ShouldAlwaysCallSetModelStateErrors(EditAppointmentModel appointment)
        {
            //given

            //when
            var result = await Controller.Create(appointment);

            //then
            _appointmentValidator.Verify(x => x.SetModelStateErrors(appointment, It.IsAny<ModelStateDictionary>()), Times.Once);
        }

        [Theory, AutoData]
        public async Task Create_ShouldReturnBadRequestStatusWithProperErrorCount_WhenModelStateHasErrors(EditAppointmentModel appointment)
        {
            //given
            Controller.ModelState.AddModelError("testError", "testErrorValue");

            //when
            var result = await Controller.Create(appointment) as ObjectResult;

            //then
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            Controller.ModelState.ErrorCount.Should().Be(1); 
        }
        
        [Theory, AutoData]
        public async Task Update_ShouldReturnOkStatusWithExpectedResult(EditAppointmentModel appointment)
        {
            //given
            _appointmentService.Setup(x => x.UpdateAppointment(It.IsAny<EditAppointmentModel>()))
                .ReturnsAsync(true)
                .Verifiable();

            //when
            var result = await Controller.Update(appointment) as ObjectResult;

            //then
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeAssignableTo<ModelStateDictionary>();
            _appointmentService.Verify();
        }

        [Theory, AutoData]
        public async Task Update_ShouldReturnBadRequestStatus_WhenUpdateActionFailed(EditAppointmentModel appointment)
        {
            //given
            _appointmentService.Setup(x => x.UpdateAppointment(It.IsAny<EditAppointmentModel>()))
                .ReturnsAsync(false)
                .Verifiable();

            //when
            var result = await Controller.Update(appointment) as ObjectResult;

            //then
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            _appointmentService.Verify();
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequestStatus_WhenAppointmentIsNull()
        {
            //given

            //when
            var result = await Controller.Update(null) as ObjectResult;

            //then
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Theory, AutoData]
        public async Task Update_ShouldReturnBadRequestStatusWithProperErrorCount_WhenModelStateHasErrors(EditAppointmentModel appointment)
        {
            //given
            Controller.ModelState.AddModelError("testError", "testErrorValue");

            //when
            var result = await Controller.Update(appointment) as ObjectResult;

            //then
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            Controller.ModelState.ErrorCount.Should().Be(1);
        }

        [Theory, AutoData]
        public async Task Update_ShouldAlwaysCallSetModelStateErrors(EditAppointmentModel appointment)
        {
            //given

            //when
            var result = await Controller.Update(appointment);

            //then
            _appointmentValidator.Verify(x => x.SetModelStateErrors(appointment, It.IsAny<ModelStateDictionary>()), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContent_WhenAppointmentWasDeleted()
        {
            //given
            _appointmentService.Setup(x => x.DeleteAppointment(It.IsAny<int>()))
                .ReturnsAsync(true);

            //when
            var result = await Controller.Delete(1) as StatusCodeResult;

            //then
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenAppointmentWasNotDeleted()
        {
            //given
            _appointmentService.Setup(x => x.DeleteAppointment(It.IsAny<int>()))
                .ReturnsAsync(false);

            //when
            var result = await Controller.Delete(1) as StatusCodeResult;

            //then
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task Delete__ShouldCallDeleteAppointmentMethod()
        {
            //given
            const int appointmentId = 11;

            //when
            await Controller.Delete(appointmentId);

            //then
            _appointmentService.Verify(x => x.DeleteAppointment(appointmentId), Times.Once);
        }
    }
}
