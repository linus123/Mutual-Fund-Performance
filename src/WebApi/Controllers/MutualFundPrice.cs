using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MutualFundPerformance.Database.MutualFund;
using MutualFundPerformance.SharedKernel;
using MutualFundPerformance.SharedKernel.Infrastructure.MutualFundData;

namespace MutualFundPerformance.WebApi.Controllers
{
    public class MutualFundPrice : Controller
    {
        private readonly IMutualFundDataGateway _mutualFundDataTableGateway;
        private readonly MutualFundPriceService _mutualFundPriceService;

        public MutualFundPrice(
            IMutualFundPerformanceDatabaseSettings mutualFundPerformanceDatabaseSettings)
        {
            _mutualFundDataTableGateway = new MutualFundDataTableGateway(
                mutualFundPerformanceDatabaseSettings);

            _mutualFundPriceService = new MutualFundPriceService(_mutualFundDataTableGateway);
        }

        public FundListModel[] GetAllFunds()
        {
            return _mutualFundPriceService.GetAllFunds();
        }

       
    }
}