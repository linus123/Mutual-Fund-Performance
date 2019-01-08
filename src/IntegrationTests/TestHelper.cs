using System;
using MutualFundPerformance.Database.HistoricalPriceData;
using MutualFundPerformance.Database.MutualFund;
using MutualFundPerformance.SharedKernel.Infrastructure.HistoricalPriceData;
using MutualFundPerformance.SharedKernel.Infrastructure.MutualFundData;
using MutualFundPerformance.WebApi.Controllers;

namespace MutualFundPerformance.IntegrationTests
{
    public class TestHelper
    {
        private MutualFundDataTableGateway mutualFundGateway;
        private IntegrationTestsSettings settings;
        private InvestmentVehicleDataTableGateway investmentVehicleDataTableGateway;
        private PriceDataTableGateway priceDataTableGateway;

        public TestHelper()
        {
            settings = new IntegrationTestsSettings();
            mutualFundGateway = new MutualFundDataTableGateway(new IntegrationTestsSettings());
            investmentVehicleDataTableGateway = new InvestmentVehicleDataTableGateway(settings);
            priceDataTableGateway = new PriceDataTableGateway(settings);
        }

        public void InsertMutualFundDto(
            MutualFundDto dto)
        {
            mutualFundGateway.Insert(new []{dto});
        }

        public void Reset(Action act)
        {
            DeleteAll();

            act();

            DeleteAll();
        }

        public MutualFundPriceController CreateController()
        {
            return new MutualFundPriceController(settings);
        }

        public void InsertPriceDto(PriceDto price)
        {
            priceDataTableGateway.Insert(new PriceDto[]{price});
        }

        public void InsertInvestmentVehicleDto(InvestmentVehicleDto dto)
        {
            investmentVehicleDataTableGateway.Insert(new InvestmentVehicleDto[]{ dto});
        }

        private void DeleteAll()
        {
            priceDataTableGateway.DeleteAll();
            mutualFundGateway.DeleteAll();
            investmentVehicleDataTableGateway.DeleteAll();
        }
    }
}