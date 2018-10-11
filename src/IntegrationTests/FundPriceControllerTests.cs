using System;
using FluentAssertions;
using MutualFundPerformance.Database.HistoricalPriceData;
using MutualFundPerformance.SharedKernel.Infrastructure.HistoricalPriceData;
using MutualFundPerformance.WebApi.Controllers;
using Xunit;

namespace MutualFundPerformance.IntegrationTests
{
    public class FundPriceControllerTests
    {
        [Fact]
        public void ByDayShouldReturnNoValueGivenSomeRandomGuid()
        {
            var mutualFundPerformanceDatabaseSettings = new IntegrationTestsSettings();
            var investmentVehicleDataTableGateway = new InvestmentVehicleDataTableGateway(
                mutualFundPerformanceDatabaseSettings);
            var priceDataTableGateway = new PriceDataTableGateway(mutualFundPerformanceDatabaseSettings);

            var fundPriceController = new FundPriceController(investmentVehicleDataTableGateway, priceDataTableGateway);

            fundPriceController.Should().NotBeNull();

            var fundId = Guid.NewGuid();

            var targetDate = new DateTime(2010, 1, 1);

            var byDay = fundPriceController.ByDay(
                fundId,
                targetDate);

            byDay.FundId.Should().Be(fundId);
            byDay.Date.Should().Be(targetDate);
            byDay.Price.Should().BeNull();
        }

        [Fact]
        public void ByDayShouldReturnNullPriceWhenNoPriceExistsForAValidFundForAGivenDay()
        {
            var mutualFundPerformanceDatabaseSettings = new IntegrationTestsSettings();

            var investmentVehicleDataTableGateway = new InvestmentVehicleDataTableGateway(
                mutualFundPerformanceDatabaseSettings);

            var priceDataTableGateway = new PriceDataTableGateway(mutualFundPerformanceDatabaseSettings);

            priceDataTableGateway.DeleteAll();
            investmentVehicleDataTableGateway.DeleteAll();

            var fundId = Guid.NewGuid();

            var investmentVehicleDto = new InvestmentVehicleDto()
            {
                InvestmentVehicleId = fundId,
                Name = "Some Name",
                ExternalId = Guid.Empty
            };

            investmentVehicleDataTableGateway.Insert(new[] { investmentVehicleDto, });

            var fundPriceController = new FundPriceController(investmentVehicleDataTableGateway, priceDataTableGateway);

            var targetDate = new DateTime(2010, 1, 1);

            var byDay = fundPriceController.ByDay(
                fundId,
                targetDate);

            byDay.FundId.Should().Be(fundId);
            byDay.Date.Should().Be(targetDate);
            byDay.Price.Should().BeNull();

            priceDataTableGateway.DeleteAll();
            investmentVehicleDataTableGateway.DeleteAll();
        }

        [Fact]
        public void ByDayShouldReturnPriceWhenPriceExistsForAValidFundForAGivenDay()
        {
            var mutualFundPerformanceDatabaseSettings = new IntegrationTestsSettings();

            var investmentVehicleDataTableGateway = new InvestmentVehicleDataTableGateway(
                mutualFundPerformanceDatabaseSettings);

            var priceDataTableGateway = new PriceDataTableGateway(mutualFundPerformanceDatabaseSettings);

            priceDataTableGateway.DeleteAll();
            investmentVehicleDataTableGateway.DeleteAll();

            var fundId = Guid.NewGuid();

            var investmentVehicleDto = new InvestmentVehicleDto()
            {
                InvestmentVehicleId = fundId,
                Name = "Some Name",
                ExternalId = Guid.Empty
            };

            investmentVehicleDataTableGateway.Insert(new[] { investmentVehicleDto, });

            var targetDate = new DateTime(2010, 1, 1);
            var price = 10M;
            priceDataTableGateway.Insert(new []
            {
                new PriceDto
                {
                    InvestmentVehicleId = fundId,
                    CloseDate = targetDate,
                    Price = price,
                }
            });

            var fundPriceController = new FundPriceController(investmentVehicleDataTableGateway, priceDataTableGateway);

            var byDay = fundPriceController.ByDay(
                fundId,
                targetDate);

            byDay.FundId.Should().Be(fundId);
            byDay.Date.Should().Be(targetDate);
            byDay.Price.Should().Be(price);

            priceDataTableGateway.DeleteAll();
            investmentVehicleDataTableGateway.DeleteAll();
        }

        /*
         *
         *{
  Date {
    Year: 2018,
    Month: 2,
    Day: 2
  },
  MutualFundIds: ["Guid1", "Guid2"]
}
         *
         */

        /*
[

  {
    Id: "Guid1",
    Name: "MF1",
    Price: 3000.2,
    OneDayReturn: 0.003,
    WeekToDate: 0.0005,
    MonthToDate: 0.0008,
    OneMonth: {
      IsValid: true,
      Value: 0.0002,
      ErrorMessage: null
    },
    ThreeMonth: {
      IsValid: false,
      Value: null,
      ErrorMessage: "Missing 1 of 3 returns"
    }
  },
    {
    Id: "Guid2",
    Name: "MF2",
    Price: 3000.2,
    OneDayReturn: 0.003,
    WeekToDate: 0.0005,
    MonthToDate: 0.0008,
    OneMonth: {
      IsValid: true,
      Value: 0.0002,
      ErrorMessage: null
    },
    ThreeMonth: {
      IsValid: false,
      Value: null,
      ErrorMessage: "Missing 1 of 3 returns"
    }
  }

]
         */

    }
}