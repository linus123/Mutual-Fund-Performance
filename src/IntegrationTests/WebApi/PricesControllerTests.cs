using System;
using System.Linq;
using Microsoft.AspNetCore.Rewrite.Internal.ApacheModRewrite;
using MutualFundPerformance.Database.MutualFund;
using MutualFundPerformance.SharedKernel.Infrastructure.MutualFundData;
using MutualFundPerformance.WebApi.Controllers;
using Xunit;

namespace MutualFundPerformance.IntegrationTests.WebApi
{
    public class PricesControllerTests
    {
        private PricesController getPriceController()
        {
            var controller = new PricesController(
                new IntegrationTestsSettings());

            return controller;
        }

    [Fact]
        
        public void FundsShouldReturnEmptyArrayWhenNoFundsAreInTheDatabase()
    {
        var controller = getPriceController();

            var funds = controller.Funds();

            Assert.Equal(0, funds.Length);
        }

        [Fact]
        public void FundsShouldReturnSingleFundAndFundNameAndIdWhenFundTableContainsSingleRecord()
        {
            var mutualFundDataTableGateway = new MutualFundDataTableGateway(
                new IntegrationTestsSettings());

            var id = Guid.NewGuid();

            var mutualFundDto = new MutualFundDto()
            {
                MutualFundId = id,
                Name = "My fund",
                Symbol = "SYM"
            };

            mutualFundDataTableGateway.DeleteAll();

            mutualFundDataTableGateway.Insert(new []{ mutualFundDto });

            var controller = getPriceController();
            var funds = controller.Funds();

            Assert.Equal(1, funds.Length);
            Assert.Equal(id, funds[0].MutualFundId);
            Assert.Equal("My fund", funds[0].Name);

            mutualFundDataTableGateway.DeleteAll();
        }



        [Fact]
        public void FundsShouldReturnMultipleFunds()
        {
            var mutualFundDataTableGateway = new MutualFundDataTableGateway(
                new IntegrationTestsSettings());

            var origionalkMutualFundDtos = new MutualFundDto[]
            {
                new MutualFundDto(){ MutualFundId = Guid.NewGuid(),Name = "My fund 1",Symbol = "SYM"},
                new MutualFundDto(){ MutualFundId = Guid.NewGuid(),Name = "My fund 3",Symbol = "CBL"},
                new MutualFundDto(){ MutualFundId = Guid.NewGuid(),Name = "My fund 2" ,Symbol = "OCT"}
            };

            mutualFundDataTableGateway.DeleteAll();

            mutualFundDataTableGateway.Insert(origionalkMutualFundDtos);

            var controller = getPriceController();
            var returnedFunds = controller.Funds();

            Assert.Equal(origionalkMutualFundDtos.Length, returnedFunds.Length);
            foreach (var fund in origionalkMutualFundDtos)
            {
                Assert.Equal(true,returnedFunds.Any(cond=> cond.MutualFundId == fund.MutualFundId));
            }
            
            mutualFundDataTableGateway.DeleteAll();
        }

    }
}