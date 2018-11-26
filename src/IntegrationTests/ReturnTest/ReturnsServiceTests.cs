using System;
using System.Linq;
using FluentAssertions;
using MutualFundPerformance.IntegrationTests.WebServiceCaller;
using MutualFundPerformance.SharedKernel.Infrastructure.HistoricalPriceData;
using Xunit;

namespace MutualFundPerformance.IntegrationTests.ReturnTest
{
    public class ReturnsServiceTests
    {
        [Fact]
        public void ShouldGetMutualFunds()
        {
            var testHelper = new TestHelper();

            testHelper.Reset(() =>
            {
                var mutualFundPrice = testHelper.CreateController();

                Guid[] idsToReturn = new Guid[1];
                int year = 0, month = 0, date = 0;


                var result = mutualFundPrice.GetFundsForIds(idsToReturn, year, month, date);

                result.Data.Length.Should().Be(0);
                result.HasError.Should().Be(true);
                result.ErrorResponse.Any(x => x.Equals("Date is invalid")).Should().Be(true);
            });
           
        }

        [Fact]
        public void ShouldValidateGuid()
        {
            var testHelper = new TestHelper();
            testHelper.Reset(() =>
            {
                var mutualFundPrice = testHelper.CreateController();

                Guid[] idsToReturn = new Guid[0];
                int year = 2017, month = 12, date = 31;


                var result = mutualFundPrice.GetFundsForIds(idsToReturn, year, month, date);

                result.Data.Length.Should().Be(0);
                result.HasError.Should().Be(true);
                result.ErrorResponse.Any(x => x.Equals("no ids are passed")).Should().Be(true);
            });
           
        }

        [Fact]
        public void ShouldValidateEmptyGuid()
        {
            var testHelper = new TestHelper();
            testHelper.Reset(() =>
            {
                var mutualFundPrice = testHelper.CreateController();

                Guid[] idsToReturn = new Guid[1];
                idsToReturn[0] = Guid.Empty; ;
                int year = 2017, month = 12, date = 31;


                var result = mutualFundPrice.GetFundsForIds(idsToReturn, year, month, date);

                result.Data.Length.Should().Be(0);
                result.HasError.Should().Be(true);
                result.ErrorResponse.Any(x => x.Equals("no ids are passed")).Should().Be(true);
            });
            
        }

        [Fact]
        public void ShouldNOtReturnDataForIdNotPresent()
        {
            var testHelper = new TestHelper();
            testHelper.Reset(() =>
            {
                var mutualFundPrice = testHelper.CreateController();

                Guid[] idsToReturn = new Guid[1];
                idsToReturn[0] = Guid.NewGuid();
                int year = 2017, month = 12, date = 31;


                var result = mutualFundPrice.GetFundsForIds(idsToReturn, year, month, date);

                result.Data.Length.Should().Be(0);
                result.HasError.Should().Be(true);
                result.ErrorResponse.Any(x => x.Equals("ids are not recognized")).Should().Be(true);
            });
           
        }

        [Fact]
        public void ShouldHandleMissingInvestmentVehicle()
        {

            var testHelper = new TestHelper();
            testHelper.Reset(() =>
            {
                var mutualFundPrice = testHelper.CreateController();

                Guid[] idsToReturn = new Guid[1];
                idsToReturn[0] = Guid.NewGuid();
                var newGuid = idsToReturn[0];
                int year = 2017, month = 12, date = 31;

                testHelper.InsertMutualFundDto(new MutualFundDtoBuilder("Mutual Fund", newGuid).Create());

                var result = mutualFundPrice.GetFundsForIds(idsToReturn, year, month, date);

                result.Data.Length.Should().Be(0);
                result.HasError.Should().Be(true);
                result.ErrorResponse.Any(x => x.Equals("Price Information is not found")).Should().Be(true);
            });
            
       }

        [Theory]
        [InlineData(2017,12,31, "No price found for 12/31/2017")]
        [InlineData(2018,1,31, "No price found for 1/31/2018")]
        public void ShouldReturnNullPricesWhenPricesAreNull(int year,int month,int day,string errorMsg)
        {
            var testHelper = new TestHelper();
            testHelper.Reset(() =>
            {
                var mutualFundPrice = testHelper.CreateController();

                Guid[] idsToReturn = new Guid[1];
                idsToReturn[0] = Guid.NewGuid();
                var newGuid = idsToReturn[0];

                testHelper.InsertMutualFundDto(new MutualFundDtoBuilder("Mutual Fund", newGuid).Create());
                testHelper.InsertInvestmentVehicleDto(new InvestmentVehicleDto()
                {
                    InvestmentVehicleId = Guid.NewGuid(),
                    Name = "VEH1",
                    ExternalId  = newGuid
                });


                var result = mutualFundPrice.GetFundsForIds(idsToReturn, year, month, day);

                result.Data.Length.Should().Be(1);
                result.HasError.Should().Be(false);
                result.Data[0].Name.Should().Be("Mutual Fund");
                result.Data[0].Price[0].Error.Should().Be(errorMsg);
                result.Data[0].Price[0].Value.Should().Be(null);

            });
        }

        [Fact]
        public void ShouldErrorOutIfOnlyOnePriceIsPresent()
        {
            var testHelper = new TestHelper();
            testHelper.Reset(() =>
            {
                var mutualFundPrice = testHelper.CreateController();

                Guid[] idsToReturn = new Guid[1];
                idsToReturn[0] = Guid.NewGuid();
                var newGuid = idsToReturn[0];

                var vehicleGuid = Guid.NewGuid();

                testHelper.InsertMutualFundDto(new MutualFundDtoBuilder("Mutual Fund", newGuid).Create());
                testHelper.InsertInvestmentVehicleDto(new InvestmentVehicleDto()
                {
                    InvestmentVehicleId = vehicleGuid,
                    Name = "VEH1",
                    ExternalId = newGuid
                });

                testHelper.InsertPriceDto(new PriceDto()
                {
                    CloseDate = new DateTime(2017,12,31),
                    InvestmentVehicleId = vehicleGuid,
                    Price =  100
                });

                int month = 12, year = 2017, day = 31;

                var result = mutualFundPrice.GetFundsForIds(idsToReturn, year, month, day);

                result.Data.Length.Should().Be(1);
                result.HasError.Should().Be(false);
                result.Data[0].Name.Should().Be("Mutual Fund");
                result.Data[0].Price[0].Error.Should().Be("No price found for 12/30/2017");
                result.Data[0].Price[0].Value.Should().Be(null);

            });
        }
        [Fact]
        public void ShouldReturnOneDayPriceInformationForDate()
        {
            var testHelper = new TestHelper();
            testHelper.Reset(() =>
            {
                var mutualFundPrice = testHelper.CreateController();

                Guid[] idsToReturn = new Guid[1];
                idsToReturn[0] = Guid.NewGuid();
                var newGuid = idsToReturn[0];

                var vehicleGuid = Guid.NewGuid();

                testHelper.InsertMutualFundDto(new MutualFundDtoBuilder("Mutual Fund", newGuid).Create());
                testHelper.InsertInvestmentVehicleDto(new InvestmentVehicleDto()
                {
                    InvestmentVehicleId = vehicleGuid,
                    Name = "VEH1",
                    ExternalId = newGuid
                });

                testHelper.InsertPriceDto(new PriceDto()
                {
                    CloseDate = new DateTime(2017, 12, 31),
                    InvestmentVehicleId = vehicleGuid,
                    Price = 100
                });

                testHelper.InsertPriceDto(new PriceDto()
                {
                    CloseDate = new DateTime(2017, 12, 30),
                    InvestmentVehicleId = vehicleGuid,
                    Price = 99
                });

                int month = 12, year = 2017, day = 31;

                var result = mutualFundPrice.GetFundsForIds(idsToReturn, year, month, day);

                result.Data.Length.Should().Be(1);
                result.HasError.Should().Be(false);
                result.Data[0].Name.Should().Be("Mutual Fund");
                result.Data[0].Price[0].Error.Should().Be("");
                result.Data[0].Price[0].Value.Should().BeLessThan(0.0m);

            });
        }

    }
}
