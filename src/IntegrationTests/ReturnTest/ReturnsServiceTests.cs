using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using FluentAssertions;
using MutualFundPerformance.Database.MutualFund;
using MutualFundPerformance.SharedKernel.Infrastructure.MutualFundData;
using MutualFundPerformance.WebApi.Controllers;
using Xunit;

namespace MutualFundPerformance.IntegrationTests.ReturnTest
{
    public class ReturnsServiceTests
    {

        private MutualFundDataTableGateway mutualFundGateway;
        private IntegrationTestsSettings settings;

        public ReturnsServiceTests()
        {
            settings = new IntegrationTestsSettings();
            mutualFundGateway = new MutualFundDataTableGateway(new IntegrationTestsSettings());
            mutualFundGateway.DeleteAll();
        }


        [Fact]
        public void ShouldGetMutualFunds()
        {
            var mutualFundPrice = new MutualFundPrice(settings);
            Guid[] idsToReturn = new Guid[1];
            int year = 0, month = 0, date = 0;


            var result = mutualFundPrice.GetFundsForIds(idsToReturn, year, month, date);

            result.Data.Length.Should().Be(0);
            result.HasError.Should().Be(true);
            result.ErrorResponse.Any(x => x.Equals("Date is invalid")).Should().Be(true);
        }


        [Fact]
        public void ShouldValidateValidDate()
        {
            var mutualFundPrice = new MutualFundPrice(settings);
            Guid[] idsToReturn = new Guid[1];
            int year = 2017, month = 12, date = 31;


            var result = mutualFundPrice.GetFundsForIds(idsToReturn, year, month, date);

            result.Data.Length.Should().Be(0);
            result.HasError.Should().Be(false);
            result.ErrorResponse.Any(x => x.Equals("Date is invalid")).Should().Be(false);
        }

        [Fact]
        public void ShouldValidateGuid()
        {
            var mutualFundPrice = new MutualFundPrice(settings);
            Guid[] idsToReturn = new Guid[0];
            int year = 2017, month = 12, date = 31;


            var result = mutualFundPrice.GetFundsForIds(idsToReturn, year, month, date);

            result.Data.Length.Should().Be(0);
            result.HasError.Should().Be(true);
            result.ErrorResponse.Any(x => x.Equals("no ids are passed")).Should().Be(true);
        }

        [Fact]
        public void ShouldValidateEmptyGuid()
        {
            var mutualFundPrice = new MutualFundPrice(settings);
            Guid[] idsToReturn = new Guid[1];
            idsToReturn[0] = Guid.Empty; ;
            int year = 2017, month = 12, date = 31;


            var result = mutualFundPrice.GetFundsForIds(idsToReturn, year, month, date);

            result.Data.Length.Should().Be(0);
            result.HasError.Should().Be(false);
            result.ErrorResponse.Any(x => x.Equals("no ids are passed")).Should().Be(false);
        }

        [Fact]
        public void ShouldNOtReturnDataForIdNotPresent()
        {
            var mutualFundPrice = new MutualFundPrice(settings);
            Guid[] idsToReturn = new Guid[1];
            idsToReturn[0] = Guid.NewGuid();
            int year = 2017, month = 12, date = 31;


            var result = mutualFundPrice.GetFundsForIds(idsToReturn, year, month, date);

            result.Data.Length.Should().Be(0);
            result.HasError.Should().Be(true);
            result.ErrorResponse.Any(x => x.Equals("ids are not recognized")).Should().Be(true);
        }
    }
}
