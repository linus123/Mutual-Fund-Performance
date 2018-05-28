using System;
using FluentAssertions;
using MutualFundPerformance.Database.HistoricalPriceData;
using MutualFundPerformance.Database.MutualFund;
using MutualFundPerformance.SharedKernel.Infrastructure.HistoricalPriceData;
using MutualFundPerformance.SharedKernel.Infrastructure.MutualFundData;
using MutualFundPerformance.WebApi.Controllers;
using Xunit;

namespace MutualFundPerformance.IntegrationTests.WepApi.Controllers
{
    public class PriceControllerTests
    {
        [Fact]
        public void ShouldReturnEmptyArrayWhenNoSymbolsAreGiven()
        {
            var integrationTestsSettings = new IntegrationTestsSettings();

            var mutualFundDataTableGateway = new MutualFundDataTableGateway(integrationTestsSettings);
            var investmentVehicleDataTableGateway = new InvestmentVehicleDataTableGateway(integrationTestsSettings);
            var priceDataTableGateway = new PriceDataTableGateway(integrationTestsSettings);

            var controller = new PriceController(
                mutualFundDataTableGateway,
                investmentVehicleDataTableGateway,
                priceDataTableGateway);

            var result = controller.GetAll(new string[0]);

            result.Should().HaveCount(0);
        }

        [Fact]
        public void ShouldReturnEmptyArrayWhenGivenSymbolIsNotFound()
        {
            var integrationTestsSettings = new IntegrationTestsSettings();

            var mutualFundDataTableGateway = new MutualFundDataTableGateway(integrationTestsSettings);
            var investmentVehicleDataTableGateway = new InvestmentVehicleDataTableGateway(integrationTestsSettings);
            var priceDataTableGateway = new PriceDataTableGateway(integrationTestsSettings);

            var controller = new PriceController(
                mutualFundDataTableGateway,
                investmentVehicleDataTableGateway,
                priceDataTableGateway);

            var symbol1 = "SYMB1";

            var result = controller.GetAll(new string[] { symbol1 });

            result.Should().HaveCount(0);
        }

        [Fact]
        public void ShouldReturnEmptyArrayWhenMutipleSymbolsAreProvedButNoneAreFound()
        {
            var integrationTestsSettings = new IntegrationTestsSettings();

            var mutualFundDataTableGateway = new MutualFundDataTableGateway(integrationTestsSettings);
            var investmentVehicleDataTableGateway = new InvestmentVehicleDataTableGateway(integrationTestsSettings);
            var priceDataTableGateway = new PriceDataTableGateway(integrationTestsSettings);

            var controller = new PriceController(
                mutualFundDataTableGateway,
                investmentVehicleDataTableGateway,
                priceDataTableGateway);

            var symbol1 = "SYMB1";
            var symbol2 = "SYMB2";

            var result = controller.GetAll(new string[] { symbol1, symbol2 });

            result.Should().HaveCount(0);
        }

        [Fact]
        public void ShouldReturnEmptyArrayWhenSymbolIsFoundButNoInvestmentVehicleIsFound()
        {
            var integrationTestsSettings = new IntegrationTestsSettings();

            var mutualFundDataTableGateway = new MutualFundDataTableGateway(integrationTestsSettings);
            var investmentVehicleDataTableGateway = new InvestmentVehicleDataTableGateway(integrationTestsSettings);
            var priceDataTableGateway = new PriceDataTableGateway(integrationTestsSettings);

            priceDataTableGateway.DeleteAll();
            investmentVehicleDataTableGateway.DeleteAll();
            mutualFundDataTableGateway.DeleteAll();

            var symbol1 = "SYMB1";

            mutualFundDataTableGateway.Insert(new []{ new MutualFundDto()
                {
                    MutualFundId = Guid.NewGuid(),
                    Name = "Some Fund",
                    Symbol = symbol1
                }
            });

            var controller = new PriceController(
                mutualFundDataTableGateway,
                investmentVehicleDataTableGateway,
                priceDataTableGateway);

            var result = controller.GetAll(new string[] { symbol1 });

            result.Should().HaveCount(0);

            priceDataTableGateway.DeleteAll();
            investmentVehicleDataTableGateway.DeleteAll();
            mutualFundDataTableGateway.DeleteAll();
        }

        [Fact]
        public void ShouldReturnEmptyArrayWhenSymbolIsAndInvestmentVehicleIsFoundButNoPricesAreFound()
        {
            var integrationTestsSettings = new IntegrationTestsSettings();

            var mutualFundDataTableGateway = new MutualFundDataTableGateway(integrationTestsSettings);
            var investmentVehicleDataTableGateway = new InvestmentVehicleDataTableGateway(integrationTestsSettings);
            var priceDataTableGateway = new PriceDataTableGateway(integrationTestsSettings);

            priceDataTableGateway.DeleteAll();
            investmentVehicleDataTableGateway.DeleteAll();
            mutualFundDataTableGateway.DeleteAll();

            var symbol1 = "SYMB1";

            var mutualFundId = Guid.NewGuid();

            mutualFundDataTableGateway.Insert(new[]{ new MutualFundDto()
                {
                    MutualFundId = mutualFundId,
                    Name = "Some Fund",
                    Symbol = symbol1
                }
            });

            investmentVehicleDataTableGateway.Insert(new []
            {
                new InvestmentVehicleDto()
                {
                    InvestmentVehicleId = Guid.NewGuid(),
                    Name = "Some Fund",
                    ExternalId = mutualFundId
                }
            });

            var controller = new PriceController(
                mutualFundDataTableGateway,
                investmentVehicleDataTableGateway,
                priceDataTableGateway);

            var result = controller.GetAll(new string[] { symbol1 });

            result.Should().HaveCount(0);

            priceDataTableGateway.DeleteAll();
            investmentVehicleDataTableGateway.DeleteAll();
            mutualFundDataTableGateway.DeleteAll();
        }

        [Fact]
        public void ShouldReturnSingleItemWhenPricesAreFound()
        {
            var integrationTestsSettings = new IntegrationTestsSettings();

            var mutualFundDataTableGateway = new MutualFundDataTableGateway(integrationTestsSettings);
            var investmentVehicleDataTableGateway = new InvestmentVehicleDataTableGateway(integrationTestsSettings);
            var priceDataTableGateway = new PriceDataTableGateway(integrationTestsSettings);

            priceDataTableGateway.DeleteAll();
            investmentVehicleDataTableGateway.DeleteAll();
            mutualFundDataTableGateway.DeleteAll();

            var symbol1 = "SYMB1";

            var mutualFundId = Guid.NewGuid();

            mutualFundDataTableGateway.Insert(new[]{ new MutualFundDto()
                {
                    MutualFundId = mutualFundId,
                    Name = "Some Fund",
                    Symbol = symbol1
                }
            });

            var investmentVehicleId = Guid.NewGuid();

            investmentVehicleDataTableGateway.Insert(new[]
            {
                new InvestmentVehicleDto()
                {
                    InvestmentVehicleId = investmentVehicleId,
                    Name = "Some Fund",
                    ExternalId = mutualFundId
                }
            });

            priceDataTableGateway.Insert(new []
            {
                new PriceDto()
                {
                    InvestmentVehicleId = investmentVehicleId,
                    CloseDate = new DateTime(2010, 1, 1),
                    Price = 10m
                }
            });

            var controller = new PriceController(
                mutualFundDataTableGateway,
                investmentVehicleDataTableGateway,
                priceDataTableGateway);

            var result = controller.GetAll(new string[] { symbol1 });

            result.Should().HaveCount(1);

            priceDataTableGateway.DeleteAll();
            investmentVehicleDataTableGateway.DeleteAll();
            mutualFundDataTableGateway.DeleteAll();
        }


    }
}