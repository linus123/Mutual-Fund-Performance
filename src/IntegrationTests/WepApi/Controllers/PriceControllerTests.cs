using System;
using FluentAssertions;
using MutualFundPerformance.Database.MutualFund;
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
            var controller = new PriceController();

            var result = controller.GetAll(new string[0]);

            result.Should().HaveCount(0);
        }

        [Fact]
        public void ShouldReturnEmptyArrayWhenGivenSymbolIsNotFound()
        {
            var controller = new PriceController();

            var symbol1 = "SYMB1";

            var result = controller.GetAll(new string[] { symbol1 });

            result.Should().HaveCount(0);
        }

        [Fact]
        public void ShouldReturnEmptyArrayWhenMutipleSymbolsAreProvedButNoneAreFound()
        {
            var controller = new PriceController();

            var symbol1 = "SYMB1";
            var symbol2 = "SYMB2";

            var result = controller.GetAll(new string[] { symbol1, symbol2 });

            result.Should().HaveCount(0);
        }

        [Fact]
        public void ShouldReturnEmptyArrayWhenSymbolIsFoundButNoInvestmentVehicleIsFound()
        {
            var mutualFundDataTableGateway = new MutualFundDataTableGateway(new IntegrationTestsSettings());

            mutualFundDataTableGateway.DeleteAll();

            var symbol1 = "SYMB1";

            mutualFundDataTableGateway.Insert(new []{ new MutualFundDto()
            {
                MutualFundId = Guid.NewGuid(),
                Name = "Some Fund",
                Symbol = symbol1
            }  });

            var controller = new PriceController();

            var result = controller.GetAll(new string[] { symbol1 });

            result.Should().HaveCount(0);

            mutualFundDataTableGateway.DeleteAll();
        }

    }
}