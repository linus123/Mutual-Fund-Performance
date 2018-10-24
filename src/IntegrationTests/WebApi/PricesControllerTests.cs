using System;
using System.Linq;
using Microsoft.AspNetCore.Rewrite.Internal.ApacheModRewrite;
using MutualFundPerformance.Database.MutualFund;
using MutualFundPerformance.SharedKernel;
using MutualFundPerformance.SharedKernel.Infrastructure.MutualFundData;
using MutualFundPerformance.WebApi.Controllers;
using Xunit;

namespace MutualFundPerformance.IntegrationTests.WebApi
{
    public class PricesControllerTests : IDisposable
    {
        private MutualFundDataTableGateway _mutualFundDataTableGateway;
        private PricesController _controller;

        public PricesControllerTests()
        {
            _mutualFundDataTableGateway = new MutualFundDataTableGateway(
                new IntegrationTestsSettings());

            _controller = getPriceController();

            _mutualFundDataTableGateway.DeleteAll();
        }

        private PricesController getPriceController()
        {
            var databaseSettings = new IntegrationTestsSettings();
            var mutualFundDataTableGateway = new MutualFundDataTableGateway(databaseSettings);
            var mutualFundPricesService = new MutualFundPricesService(mutualFundDataTableGateway);
            var controller = new PricesController( mutualFundPricesService);

            return controller;
        }

        [Fact]
        
        public void FundsShouldReturnEmptyArrayWhenNoFundsAreInTheDatabase()
        {
            var funds = _controller.Funds();

            Assert.Equal(0, funds.Length);
        }

        [Fact]
        public void FundsShouldReturnSingleFundAndFundNameAndIdWhenFundTableContainsSingleRecord()
        {

            var id = Guid.NewGuid();

            var mutualFundDto = new MutualFundDto()
            {
                MutualFundId = id,
                Name = "My fund",
                Symbol = "SYM"
            };

            _mutualFundDataTableGateway.Insert(new []{ mutualFundDto });

            var funds = _controller.Funds();

            Assert.Equal(1, funds.Length);
            Assert.Equal(id, funds[0].MutualFundId);
            Assert.Equal("My fund", funds[0].Name);
        }

        [Fact]
        public void FundsShouldBeSortedMultipleFunds()
        {
            var origionalkMutualFundDtos = new MutualFundDto[]
            {
                new MutualFundDto(){ MutualFundId = Guid.NewGuid(),Name = "My fund 1",Symbol = "SYM"},
                new MutualFundDto(){ MutualFundId = Guid.NewGuid(),Name = "My fund 3",Symbol = "CBL"},
                new MutualFundDto(){ MutualFundId = Guid.NewGuid(),Name = "My fund 2" ,Symbol = "OCT"}
            };

            _mutualFundDataTableGateway.Insert(origionalkMutualFundDtos);

            var returnedFunds = _controller.Funds();
            
            Assert.Equal(origionalkMutualFundDtos.Length, returnedFunds.Length);
            Assert.Equal(origionalkMutualFundDtos[0].MutualFundId, returnedFunds[0].MutualFundId);
            Assert.Equal(origionalkMutualFundDtos[1].MutualFundId, returnedFunds[2].MutualFundId);
            Assert.Equal(origionalkMutualFundDtos[2].MutualFundId, returnedFunds[1].MutualFundId);
        }

        [Fact]
        public void FundsShouldReturnMultipleFunds()
        {
            var origionalkMutualFundDtos = new MutualFundDto[]
            {
                new MutualFundDto(){ MutualFundId = Guid.NewGuid(),Name = "My fund 1",Symbol = "SYM"},
                new MutualFundDto(){ MutualFundId = Guid.NewGuid(),Name = "My fund 3",Symbol = "CBL"},
                new MutualFundDto(){ MutualFundId = Guid.NewGuid(),Name = "My fund 2" ,Symbol = "OCT"}
            };

            _mutualFundDataTableGateway.Insert(origionalkMutualFundDtos);

            var returnedFunds = _controller.Funds();

            Assert.Equal(origionalkMutualFundDtos.Length, returnedFunds.Length);
            foreach (var fund in origionalkMutualFundDtos)
            {
                Assert.Equal(true,returnedFunds.Any(cond=> cond.MutualFundId == fund.MutualFundId));
            }
        }

        public void Dispose()
        {
            _mutualFundDataTableGateway.DeleteAll();
        }
    }
}