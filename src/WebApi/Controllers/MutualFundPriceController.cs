using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MutualFundPerformance.Database.HistoricalPriceData;
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
        private PriceDataTableGateway _priceDataTableGateway;

        public MutualFundPriceController(
            IMutualFundPerformanceDatabaseSettings mutualFundPerformanceDatabaseSettings,
            IInvestmentVehicleDataTableGateway investmentVehicleDataTableGateway)
        {
            _investmentVehicleDataTableGateway = investmentVehicleDataTableGateway;
            _mutualFundDataTableGateway = new MutualFundDataTableGateway(
                mutualFundPerformanceDatabaseSettings);

            _mutualFundPriceService = new MutualFundPriceService(_mutualFundDataTableGateway);
            _priceDataTableGateway = new PriceDataTableGateway(mutualFundPerformanceDatabaseSettings);
        }

        public FundListModel[] GetAllFunds()
        {
            return _mutualFundPriceService.GetAllFunds();
        }

        public ReturnModel GetFundsForIds(
            Guid[] idsToReturn,
            int year, int month, int day)
        {
            DateTime endDate = DateTime.MinValue; 

            try
            {
                endDate = new DateTime(year, month, day);
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

            var priceDtos = _priceDataTableGateway.GetAll();

            var priceDayOf = priceDtos.FirstOrDefault(p => p.InvestmentVehicleId == investmentVehicleDto.InvestmentVehicleId && p.CloseDate == endDate);

            if (priceDayOf == null)
            {
                var formattedDate = FormattedDate(endDate);

                var mutualFundWithPerformance = new MutualFundWithPerformance()
                {
                    Name = mutualFundDto.Name,
                    Price = new[]
                    {
                        new PriceModel()
                        {
                            Error = $"No price found for {formattedDate}",
                            Value = null
                        },
                    }
                };

                return new ReturnModel()
                {
                    Data = new MutualFundWithPerformance[] { mutualFundWithPerformance },
                    HasError = false,
                    ErrorResponse = new string[0]
                };
            }
            else
            {
                var formattedDate = FormattedDate(endDate.AddDays(-1));

                var mutualFundWithPerformance = new MutualFundWithPerformance()
                {
                    Name = mutualFundDto.Name,
                    Price = new[]
                    {
                        new PriceModel()
                        {
                            Error = $"No price found for {formattedDate}",
                            Value = null
                        },
                    }
                };

                return new ReturnModel()
                {
                    Data = new MutualFundWithPerformance[] { mutualFundWithPerformance },
                    HasError = false,
                    ErrorResponse = new string[0]
                };
            }
        }

        private static string FormattedDate(DateTime endDate)
        {
            return $"{endDate.Month}/{endDate.Day}/{endDate.Year}";
        }

        private static ReturnModel CreateModelWithError(
            string errorMessage)
        {
            return new ReturnModel()
            {
                Data = new MutualFundWithPerformance[0],
                HasError = true,
                ErrorResponse = new[] {errorMessage}
            };
        }

        public class ReturnModel
        {
            public MutualFundWithPerformance[] Data { get; set; }
            public bool HasError { get; set; }
            public string[] ErrorResponse { get; set; }
        }

        public class MutualFundWithPerformance
        {
            public string Name { get; set; }
            public PriceModel[] Price { get; set; }
        }

        public class PriceModel
        {
            public decimal? Value { get; set; }
            public string Error { get; set; }
        }
    }
}