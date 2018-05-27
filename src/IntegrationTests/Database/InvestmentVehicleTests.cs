using System;
using FluentAssertions;
using MutualFundPerformance.Database.HistoricalPriceData;
using MutualFundPerformance.SharedKernel.Infrastructure.HistoricalPriceData;
using Xunit;

namespace MutualFundPerformance.IntegrationTests.Database
{
    public class InvestmentVehicleTests
    {
        private readonly IntegrationTestsSettings _mutualFundPerformanceDatabaseSettings;

        public InvestmentVehicleTests()
        {
            _mutualFundPerformanceDatabaseSettings = new IntegrationTestsSettings();
        }

        [Fact]
        public void InvestmentVehicleDataTableGatewayShouldPresistData()
        {

            var investmentVehicleDataTableGateway = new InvestmentVehicleDataTableGateway(_mutualFundPerformanceDatabaseSettings);

            investmentVehicleDataTableGateway.DeleteAll();

            var investmentVehicleId = Guid.NewGuid();
            var externalId = Guid.NewGuid();

            investmentVehicleDataTableGateway.Insert(new []
            {
                new InvestmentVehicleDto()
                {
                    InvestmentVehicleId = investmentVehicleId,
                    Name = "Foo",
                    ExternalId = externalId
                }
            });

            var investmentVehicleDtos = investmentVehicleDataTableGateway.GetAll();

            investmentVehicleDtos.Should().HaveCount(1);

            investmentVehicleDtos[0].InvestmentVehicleId.Should().Be(investmentVehicleId);
            investmentVehicleDtos[0].Name.Should().Be("Foo");
            investmentVehicleDtos[0].ExternalId.Should().Be(externalId);

            investmentVehicleDataTableGateway.DeleteAll();
        }

        [Fact]
        public void PriceDataTableGatewayShouldPresistData()
        {
            var investmentVehicleDataTableGateway = new InvestmentVehicleDataTableGateway(_mutualFundPerformanceDatabaseSettings);
            var priceDataTableGateway = new PriceDataTableGateway(_mutualFundPerformanceDatabaseSettings);

            priceDataTableGateway.DeleteAll();
            investmentVehicleDataTableGateway.DeleteAll();

            var investmentVehicleId = Guid.NewGuid();
            var externalId = Guid.NewGuid();

            investmentVehicleDataTableGateway.Insert(new[]
            {
                new InvestmentVehicleDto()
                {
                    InvestmentVehicleId = investmentVehicleId,
                    Name = "Foo",
                    ExternalId = externalId
                }
            });

            priceDataTableGateway.Insert(new []
            {
                new PriceDto()
                {
                    InvestmentVehicleId = investmentVehicleId,
                    CloseDate = new DateTime(2010, 1, 1),
                    Price = 32.0m
                }
            });

            var priceDtos = priceDataTableGateway.GetAll();

            priceDtos.Should().HaveCount(1);
            priceDtos[0].InvestmentVehicleId.Should().Be(investmentVehicleId);
            priceDtos[0].CloseDate.Should().Be(new DateTime(2010, 1, 1));
            priceDtos[0].Price.Should().Be(32.0m);

            priceDataTableGateway.DeleteAll();
            investmentVehicleDataTableGateway.DeleteAll();
        }

    }
}