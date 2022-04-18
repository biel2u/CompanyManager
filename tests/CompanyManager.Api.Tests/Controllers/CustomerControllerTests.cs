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
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CompanyManager.Api.Tests.Controllers
{
    public class CustomerControllerTests : ControllerTestsBase<CustomerController>
    {
        private readonly Mock<ICustomerService> _customerService;
        private readonly Mock<ICustomerValidator> _customerValidator;

        public CustomerControllerTests()
        {
            _customerService = Mocker.GetMock<ICustomerService>();
            _customerValidator = Mocker.GetMock<ICustomerValidator>();
        }

        [Fact]
        public async Task GetByValue_ShouldCallSearchCustomers()
        {
            //given
            const string searchValue = "testSearch";

            //when
            await Controller.GetByValue(searchValue);

            //then
            _customerService.Verify(x => x.SearchCustomers(searchValue), Times.Once);
        }

        [Theory, AutoData]
        public async Task GetByValue_ShouldReturnOkStatusWithExpectedResult(List<string> customers)
        {
            //given
            _customerService.Setup(x => x.SearchCustomers(It.IsAny<string>()))
                .ReturnsAsync(customers);

            //when
            var result = await Controller.GetByValue(default) as OkObjectResult;

            //then
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeAssignableTo<List<string>>();
            var value = result.Value as List<string>;
            value.Should().HaveCount(customers.Count);
        }

        [Theory, AutoData]
        public async Task Create_ShouldReturnCreatedStatusWithExpectedResult(EditCustomerModel customer)
        {
            //given
            _customerService.Setup(x => x.AddCustomer(It.IsAny<EditCustomerModel>()))
                .Verifiable();

            //when
            var result = await Controller.Create(customer) as ObjectResult;

            //then
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
            result.Value.Should().BeAssignableTo<ModelStateDictionary>();
            _customerService.Verify();
        }       

        [Fact]
        public async Task Create_ShouldReturnBadRequestStatus_WhenCustomerIsNull()
        {
            //given

            //when
            var result = await Controller.Create(null) as ObjectResult;

            //then
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Theory, AutoData]
        public async Task Create_ShouldAlwaysCallSetModelStateErrors(EditCustomerModel customer)
        {
            //given

            //when
            var result = await Controller.Create(customer);

            //then
            _customerValidator.Verify(x => x.SetModelStateErrors(customer, It.IsAny<ModelStateDictionary>()), Times.Once);
        }

        [Theory, AutoData]
        public async Task Create_ShouldReturnBadRequestStatusWithProperErrorCount_WhenModelStateHasErrors(EditCustomerModel customer)
        {
            //given
            Controller.ModelState.AddModelError("testError", "testErrorValue");

            //when
            var result = await Controller.Create(customer) as ObjectResult;

            //then
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            Controller.ModelState.ErrorCount.Should().Be(1);
        }
    }
}
