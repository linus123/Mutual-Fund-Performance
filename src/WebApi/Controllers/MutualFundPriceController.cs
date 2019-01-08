using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MutualFundPerformance.Database.HistoricalPriceData;
using MutualFundPerformance.Database.MutualFund;
using MutualFundPerformance.SharedKernel;
using MutualFundPerformance.SharedKernel.Infrastructure.HistoricalPriceData;
using MutualFundPerformance.SharedKernel.Infrastructure.MutualFundData;
using MutualFundPerformance.SharedKernel.Performance;

namespace MutualFundPerformance.WebApi.Controllers
{
    public class MutualFundPriceController : Controller
    {
        private readonly MutualFundServiceForUI _mutualFundPriceService;
        private readonly PerformanceService _performanceService;
        private readonly InvestmentVehicleDataTableGateway _investmentVehicleDataTableGateway;
        private readonly MutualFundDataTableGateway _mutualFundDataTableGateway;
        private readonly PriceDataTableGateway _priceDataTableGateway;


        public MutualFundPriceController()
        {
            _investmentVehicleDataTableGateway = new InvestmentVehicleDataTableGateway(new WebApiSettings(Startup.Configuration));
            _mutualFundDataTableGateway = new MutualFundDataTableGateway(
                new WebApiSettings(Startup.Configuration));

            _mutualFundPriceService = new MutualFundServiceForUI(_mutualFundDataTableGateway);
            _priceDataTableGateway = new PriceDataTableGateway(new WebApiSettings(Startup.Configuration));

            _performanceService = new PerformanceService(_priceDataTableGateway,_mutualFundDataTableGateway,_investmentVehicleDataTableGateway);
        }

        public MutualFundPriceController(IMutualFundPerformanceDatabaseSettings settings)
        {
            _investmentVehicleDataTableGateway = new InvestmentVehicleDataTableGateway(settings);
            _mutualFundDataTableGateway = new MutualFundDataTableGateway(
                settings);

            _mutualFundPriceService = new MutualFundServiceForUI(_mutualFundDataTableGateway);
            _priceDataTableGateway = new PriceDataTableGateway(settings);

            _performanceService = new PerformanceService(_priceDataTableGateway, _mutualFundDataTableGateway, _investmentVehicleDataTableGateway);
        }

        public FundListModel[] GetAllFunds()
        {
            return _mutualFundPriceService.GetAllFunds();
        }

        public PerformanceService.ReturnModel GetFundsForIds(
            Guid[] idsToReturn,
            int year, int month, int day)
        {
            return _performanceService.GetMutualFundsForIds(idsToReturn, year, month, day);
        }

       

       

    }
}