using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using FluentAssertions;
using MutualFundPerformance.SharedKernel.Infrastructure.HistoricalPriceData;
using Xunit;


namespace MutualFundPerformance.IntegrationTests
{
    public class ServiceTests 
    {
        [Fact]  
        public void ShouldGetNoFunds()
        {
            var testHelper = new TestHelper();

            testHelper.Reset(() =>
            {
                var mutualFundPrice = testHelper.CreateController();
                mutualFundPrice.GetAllFunds().Length.Should().Be(0);
            });

        }
       
        [Fact]
        public void ShouldGetInsertedDataBack()
        {
            var testHelper = new TestHelper();

            testHelper.Reset(() =>
            {
                testHelper.InsertMutualFundDto(
                    new MutualFundDtoBuilder("Mutual Fund 1").Create()
                    );

                var mutualFundPrice = testHelper.CreateController();

                mutualFundPrice.GetAllFunds().Length.Should().Be(1);
                mutualFundPrice.GetAllFunds().Single().Name.Should().Be("Mutual Fund 1");
            });

            
        }

        [Fact]
        public void ShouldReturnFundsInOrder()
        {

            var testHelper = new TestHelper();

            testHelper.Reset(() =>
            {
                testHelper.InsertMutualFundDto(
                    new MutualFundDtoBuilder("Mutual Fund 1").Create()
                );
                testHelper.InsertMutualFundDto(
                    new MutualFundDtoBuilder("A Mutual Fund 1").Create()
                );

                testHelper.InsertMutualFundDto(
                    new MutualFundDtoBuilder("B Mutual Fund 1").Create()
                );


                var mutualFundPrice = testHelper.CreateController();

                mutualFundPrice.GetAllFunds().Length.Should().Be(3);
                mutualFundPrice.GetAllFunds()[0].Name.Should().Be("A Mutual Fund 1");
                mutualFundPrice.GetAllFunds()[1].Name.Should().Be("B Mutual Fund 1");
                mutualFundPrice.GetAllFunds()[2].Name.Should().Be("Mutual Fund 1");
            });

          
          
           
        }

        [Fact]
        public void ShouldReturnMutualFundID()
        {
            var testHelper = new TestHelper();

            testHelper.Reset(() =>
            {
                Guid newGuid = Guid.NewGuid();
                testHelper.InsertMutualFundDto(
                
                    new MutualFundDtoBuilder("Mutual Fund 1",newGuid).Create()
                );

                var mutualFundPrice = testHelper.CreateController();

                var fundListModels = mutualFundPrice.GetAllFunds();

                fundListModels.Length.Should().Be(1);
                fundListModels[0].Id.Should().Be(newGuid);
        });

        }

      
    }
}
