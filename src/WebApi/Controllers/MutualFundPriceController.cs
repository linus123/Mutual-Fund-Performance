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
        

        public MutualFundPriceController(
            Pei)
        {
            _investmentVehicleDataTableGateway = investmentVehicleDataTableGateway;
            _mutualFundDataTableGateway = new MutualFundDataTableGateway(
                mutualFundPerformanceDatabaseSettings);

            _mutualFundPriceService = new MutualFundServiceForUI(_mutualFundDataTableGateway);
            _priceDataTableGateway = new PriceDataTableGateway(mutualFundPerformanceDatabaseSettings);
        }

        public FundListModel[] GetAllFunds()
        {
            return _mutualFundPriceService.GetAllFunds();
        }

        public PerformanceService.ReturnModel GetFundsForIds(
            Guid[] idsToReturn,
            int year, int month, int day)
        {
            

          

          
            
        }

       

       

    }
}