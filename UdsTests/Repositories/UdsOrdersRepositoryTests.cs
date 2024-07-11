using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Uds.Database;
using Uds.Models;
using Uds.Repositories;

namespace UdsTests.Repositories
{
    [TestFixture]
    public class UdsOrdersRepositoryTests
    {
        private WebApplication mockApp;

        [SetUp]
        public void SetUp()
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder();
            builder.Services.AddDbContext<DbConnection>();

            builder.Services.AddScoped<UdsOrdersRepository>();

            mockApp = builder.Build();
        }

        private UdsOrdersRepository CreateUdsOrdersRepository()
        {
            return mockApp.Services.GetService<UdsOrdersRepository>();
        }

        [Test]
        public void CreateUdsOrdersRepository_NotNull()
        {
            UdsOrdersRepository udsOrdersRepository = CreateUdsOrdersRepository();
            Assert.That(udsOrdersRepository != null);
        }

        [Test]
        public void GetUdsOrders_ExistingOrdersInDb_NonEmptyList()

        {
            // Arrange
            UdsOrdersRepository udsOrdersRepository = this.CreateUdsOrdersRepository();

            // Act
            List<UdsOrderModel> result = udsOrdersRepository.GetUdsOrders();

            // Assert
            Assert.That(result.Count > 0);
        }

        private bool OrderValid(UdsOrderModel model)
        {
            return model.Id > 0 &&
                model.BookingSite != null &&
                model.Origin.Length == 3 &&
                model.Destination.Length == 3 &&
                model.Schedule != null;
        }


        [Test]
        public void GetUdsOrders_ExistingOrdersInDb_ValidOrders()

        {
            // Arrange
            UdsOrdersRepository udsOrdersRepository = this.CreateUdsOrdersRepository();

            // Act
            List<UdsOrderModel> result = udsOrdersRepository.GetUdsOrders();

            // Assert
            Assert.That(result.All(OrderValid));
        }

        [Test]
        [TestCase(-100)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(888)]
        [TestCase(6)]
        [TestCase(7)]
        [TestCase(8)]
        [TestCase(10)]
        public void GetOrder_OrderExists_ValidOrder(int id)
        {
            // Arrange
            UdsOrdersRepository udsOrdersRepository = this.CreateUdsOrdersRepository();

            // Act
            UdsOrderModel result = udsOrdersRepository.GetOrder(id);

            // Assert
            if (result == null)
            {
                Assert.Warn($"No order with id {id}");
                return;
            }

            Assert.That(result.Id == id);
            Assert.That(OrderValid(result));
        }
    }
}
