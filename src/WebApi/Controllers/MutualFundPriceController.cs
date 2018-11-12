using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MutualFundPerformance.Database.MutualFund;
using MutualFundPerformance.SharedKernel;
using MutualFundPerformance.SharedKernel.Infrastructure.HistoricalPriceData;
using MutualFundPerformance.SharedKernel.Infrastructure.MutualFundData;

namespace MutualFundPerformance.WebApi.Controllers
{
    public class MutualFundPriceController : Controller
    {
        private readonly MutualFundPriceService _mutualFundPriceService;

        private readonly IMutualFundDataGateway _mutualFundDataTableGateway;
        private readonly IInvestmentVehicleDataTableGateway _investmentVehicleDataTableGateway;

        public MutualFundPriceController(
            IMutualFundPerformanceDatabaseSettings mutualFundPerformanceDatabaseSettings,
            IInvestmentVehicleDataTableGateway investmentVehicleDataTableGateway)
        {
            _investmentVehicleDataTableGateway = investmentVehicleDataTableGateway;
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

            idsToReturn = idsToReturn.Where(id => id != Guid.Empty).ToArray();

            if (idsToReturn.Length <= 0)
            {
                return CreateModelWithError("no ids are passed");
            }

            var mutualFundDtos = _mutualFundDataTableGateway.GetAll();

            var mutualFundDto = mutualFundDtos.FirstOrDefault(m => idsToReturn.Any(id => id == m.MutualFundId));

            if (mutualFundDto == null)
            {
                return CreateModelWithError("ids are not recognized");
            }

            var investmentVehicleDtos = _investmentVehicleDataTableGateway.GetAll();

            var investmentVehicleDto = investmentVehicleDtos.FirstOrDefault(d => d.ExternalId == mutualFundDto.MutualFundId);

            if (investmentVehicleDto == null)
            {
                return CreateModelWithError("Price Information is not found");
            }

            return new ReturnModel()
            {
                Data = new object[1],
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