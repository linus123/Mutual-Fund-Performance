using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MutualFundPerformance.SharedKernel.Infrastructure.HistoricalPriceData;

namespace MutualFundPerformance.SharedKernel.Performance
{
    public class PerformanceService
    {
        private IPriceDataTableGateway _priceDataTableGateway;
        private readonly IMutualFundDataGateway _mutualFundDataTableGateway;
        private readonly IInvestmentVehicleDataTableGateway _investmentVehicleDataTableGateway;

        public PerformanceService(IPriceDataTableGateway priceDataTableGateway,
            IMutualFundDataGateway mutualFundDataGateway,
            IInvestmentVehicleDataTableGateway investmentVehicleDataTableGateway)
        {
            _priceDataTableGateway = priceDataTableGateway;
            _mutualFundDataTableGateway = mutualFundDataGateway;
            _investmentVehicleDataTableGateway = investmentVehicleDataTableGateway;
        }

        public ReturnModel GetMutualFundsForIds(Guid[] idsToReturn,
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

            var investmentVehicleDto =
                investmentVehicleDtos.FirstOrDefault(d => d.ExternalId == mutualFundDto.MutualFundId);

            if (investmentVehicleDto == null)
            {
                return CreateModelWithError("Price Information is not found");
            }

            var priceDtos = _priceDataTableGateway.GetAll();

            var priceDayOf = priceDtos.FirstOrDefault(p =>
                p.InvestmentVehicleId == investmentVehicleDto.InvestmentVehicleId && p.CloseDate == endDate);

            if (priceDayOf == null)
            {
                var formattedDate = FormattedDate(endDate);

                var mutualFundWithPerformance = new MutualFundWithPerformance()
                {
                    Name = mutualFundDto.Name,
                    Price = new[]
                    {
                        new Returns()
                        {
                            Error = $"No price found for {formattedDate}",
                            Value = null
                        },
                    }
                };

                return new ReturnModel()
                {
                    Data = new MutualFundWithPerformance[] {mutualFundWithPerformance},
                    HasError = false,
                    ErrorResponse = new string[0]
                };
            }
            else
            {
                var formattedDate = FormattedDate(endDate.AddDays(-1));

                var pricePrevDay = priceDtos.FirstOrDefault(p =>
                    p.InvestmentVehicleId == investmentVehicleDto.InvestmentVehicleId &&
                    p.CloseDate == endDate.AddDays(-1));

                var mutualFundWithPerformance = new MutualFundWithPerformance();

                if (pricePrevDay == null)
                {

                    mutualFundWithPerformance = new MutualFundWithPerformance()
                    {
                        Name = mutualFundDto.Name,
                        Price = new[]
                        {
                            new Returns()
                            {
                                Error = $"No price found for {formattedDate}",
                                Value = null
                            },
                        }
                    };

                    return new ReturnModel()
                    {
                        Data = new MutualFundWithPerformance[] {mutualFundWithPerformance},
                        HasError = false,
                        ErrorResponse = new string[0]
                    };
                }

                mutualFundWithPerformance = new MutualFundWithPerformance()
                {
                    Name = mutualFundDto.Name,
                    Price = new[]
                    {
                        new Returns()
                        {
                            Error = "",
                            Value = -1.0m
                        },
                    }
                };


                return new ReturnModel()
                {
                    Data = new MutualFundWithPerformance[] {mutualFundWithPerformance},
                    HasError = false,
                    ErrorResponse = new string[0]
                };
            }


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

        private static string FormattedDate(DateTime endDate)
        {
            return $"{endDate.Month}/{endDate.Day}/{endDate.Year}";
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
            public Returns[] Price { get; set; }
        }

        public class Returns
        {
            public decimal? Value { get; set; }
            public string Error { get; set; }
        }
    }
}
