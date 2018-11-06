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


        public ReturnModel GetFundsForIds(
            Guid[] idsToReturn,
            int year, int month, int day)
        {
            try
            {
                new DateTime(year, month, day);
            }
            catch (Exception e)
            {
                return CreateModelWithError("Date is invalid");
            }

            if (idsToReturn.Length <= 0)
            {
                return CreateModelWithError("no ids are passed");
            }

            if (idsToReturn.Any(d => d != Guid.Empty))
            {
                return CreateModelWithError("ids are not recognized");
            }

            return new ReturnModel()
            {
                Data = new object[0],
                HasError = false,
                ErrorResponse = new string[0]
            };
        }

        private static ReturnModel CreateModelWithError(
            string errorMessage)
        {
            return new ReturnModel()
            {
                Data = new object[0],
                HasError = true,
                ErrorResponse = new[] {errorMessage}
            };
        }

        public class ReturnModel
        {
            public object[] Data { get; set; }
            public bool HasError { get; set; }
            public string[] ErrorResponse { get; set; }
        }
    }
}