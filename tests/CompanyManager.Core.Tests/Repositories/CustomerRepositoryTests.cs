using AutoFixture;
using AutoFixture.Xunit2;
using CompanyManager.Core.Data;
using CompanyManager.Core.Models;
using CompanyManager.Core.Repositories;
using Duende.IdentityServer.EntityFramework.Options;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace CompanyManager.Core.Tests.Repositories
{
    public class CustomerRepositoryTests
    {
        private static readonly Fixture _fixture = new Fixture();

        private readonly CustomerRepository _repository;
        private readonly Mock<ApplicationDbContext> _dbContextMock;

        public CustomerRepositoryTests()
        {
            _fixture.Customize<Customer>(x => x.Without(y => y.Consent));
            _fixture.Customize<Customer>(x => x.Without(y => y.Photos));
            _dbContextMock = new Mock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options, It.IsAny<IOptions<OperationalStoreOptions>>());
            _repository = new CustomerRepository(_dbContextMock.Object);
        }

        [Theory, AutoData]
        public async Task GetCustomersByName_ShouldReturn5CustomersOrderedBySurnameAsc_WhenCustomerWithGivenNameExists(string searchValue)
        {
            //given
            var customers = _fixture.Build<Customer>()
                .Without(x => x.Consent)
                .Without(x => x.Photos)
                .With(x => x.Name, searchValue)
                .CreateMany(10);

            _dbContextMock.Setup(x => x.Customers).Returns(customers.GetMockDbSetObject());

            //when
            var result = await _repository.GetCustomersByName(searchValue);

            //then
            var surnameOrderAsc = customers.OrderBy(x => x.Surname).First();
            result.First().Should().Be(surnameOrderAsc);
            result.Should().Contain(x => x.Name == searchValue);
            result.Should().HaveCount(5);
        }

        [Theory, AutoData]
        public async Task GetCustomersByName_ShouldReturn5Customers_WhenCustomerWithGivenSurnameExists(string searchValue)
        {
            //given
            var customer = _fixture.Build<Customer>()
                .Without(x => x.Consent)
                .Without(x => x.Photos)
                .With(x => x.Surname, searchValue)
                .CreateMany(10);

            _dbContextMock.Setup(x => x.Customers).Returns(customer.GetMockDbSetObject());

            //when
            var result = await _repository.GetCustomersByName(searchValue);

            //then
            result.Should().Contain(x => x.Surname == searchValue);
            result.Should().HaveCount(5);

        }

        [Theory, AutoData]
        public async Task GetCustomersByName_ShouldReturnEmptyList_WhenNoCustomersWithGivenNameAndSurnameExists(string searchValue)
        {
            //given
            var customer = _fixture.Build<Customer>()
                .Without(x => x.Consent)
                .Without(x => x.Photos)
                .With(x => x.Name, "name123")
                .With(x => x.Surname, "surname123")
                .Create();

            _dbContextMock.Setup(x => x.Customers).Returns(new[] { customer }.GetMockDbSetObject());

            //when
            var result = await _repository.GetCustomersByName(searchValue);

            //then
            result.Should().HaveCount(0);
        }

        [Theory, AutoData]
        public async Task GetCustomerByPhone_ShouldReturn5CustomersOrderedBySurnameAsc_WhenCustomerWithGivenPhoneExists(string searchValue)
        {
            //given
            var customers = _fixture.Build<Customer>()
                .Without(x => x.Consent)
                .Without(x => x.Photos)
                .With(x => x.Phone, searchValue)
                .CreateMany(10);

            _dbContextMock.Setup(x => x.Customers).Returns(customers.GetMockDbSetObject());

            //when
            var result = await _repository.GetCustomersByPhone(searchValue);

            //then
            var surnameOrderAsc = customers.OrderBy(x => x.Surname).First();
            result.First().Should().Be(surnameOrderAsc);
            result.Should().Contain(x => x.Phone == searchValue);
            result.Should().HaveCount(5);
        }

        [Theory, AutoData]
        public async Task GetCustomersByPhone_ShouldReturnEmptyList_WhenNoCustomerWithGivenPhoneExists(string searchValue)
        {
            //given
            var customer = _fixture.Build<Customer>()
                .Without(x => x.Consent)
                .Without(x => x.Photos)
                .With(x => x.Phone, "1234566799")
                .Create();

            _dbContextMock.Setup(x => x.Customers).Returns(new[] { customer }.GetMockDbSetObject());

            //when
            var result = await _repository.GetCustomersByPhone(searchValue);

            //then
            result.Should().HaveCount(0);
        }

        [Theory, AutoData]
        public async Task GetCustomerByPhone_ShouldReturnExpectedCustomer_WhenPhoneExists(string phone)
        {
            //given
            var customer = _fixture.Build<Customer>()
                .Without(x => x.Consent)
                .Without(x => x.Photos)
                .With(x => x.Phone, phone)
                .Create();

            _dbContextMock.Setup(x => x.Customers).Returns(new[] { customer }.GetMockDbSetObject());

            //when
            var result = await _repository.GetCustomerByPhone(phone);

            //then
            result?.Phone.Should().Be(phone);
        }

        [Theory, AutoData]
        public async Task GetCustomerByPhone_ShouldReturnNull_WhenPhoneDoesNotExist(string phone)
        {
            //given
            var customer = _fixture.Build<Customer>()
                .Without(x => x.Consent)
                .Without(x => x.Photos)
                .With(x => x.Phone, "1234567899")
                .Create();

            _dbContextMock.Setup(x => x.Customers).Returns(new[] { customer }.GetMockDbSetObject());

            //when
            var result = await _repository.GetCustomerByPhone(phone);

            //then
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddCustomer_ShouldAddAndSaveCustomer()
        {
            //given
            var customer = _fixture.Build<Customer>()
                .Without(x => x.Consent)
                .Without(x => x.Photos)
                .CreateMany(10);

            _dbContextMock.Setup(x => x.Customers).Returns(customer.GetMockDbSetObject());

            var newCustomer = new Customer
            {
                Surname = "testSurname",               
            };

            //when
            await _repository.AddCustomer(newCustomer);

            //then
            _dbContextMock.Verify(x => x.Customers.AddAsync(It.Is<Customer>(y => y.Surname == newCustomer.Surname), It.IsAny<CancellationToken>()), Times.Once);
            _dbContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
