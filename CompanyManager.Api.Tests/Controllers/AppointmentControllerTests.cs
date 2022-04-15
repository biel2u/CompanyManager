using AutoFixture.Xunit2;
using CompanyManager.Api.Controllers;
using CompanyManager.Core.Services;
using CompanyManager.Core.Validators;
using CompanyManager.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                .ReturnsAsync(appointments);

            //when
            var result = await Controller.GetInRange(new AppointmentsRange()) as OkObjectResult;

            //then
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeAssignableTo<List<DisplayAppointmentModel>>();
            var value = result.Value as List<DisplayAppointmentModel>;
            value.Should().HaveCount(appointments.Count());
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
            result.Value.Should().BeAssignableTo<EditAppointmentModel>();
        }
    }
}
