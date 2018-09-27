using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MutualFundPerformance.Database.HistoricalPriceData;
using MutualFundPerformance.SharedKernel.Infrastructure.HistoricalPriceData;
using MutualFundPerformance.WebApi.Models;

namespace MutualFundPerformance.WebApi.Controllers
{
    public class FundPriceController : Controller
    {
        private readonly InvestmentVehicleDataTableGateway _investmentVehicleDataTableGateway;
        private readonly PriceDataTableGateway _priceDataTableGateway;

        public FundPriceController(
            InvestmentVehicleDataTableGateway investmentVehicleDataTableGateway,
            PriceDataTableGateway priceDataTableGateway)
        {
            _priceDataTableGateway = priceDataTableGateway;
            _investmentVehicleDataTableGateway = investmentVehicleDataTableGateway;
        }

        public FundPrice ByDay(
            Guid fundId,
            DateTime date)
        {
            var fund = GetInvestmentVehicleDto(fundId);

            if (fund == null) return CreateNoPriceResult(fundId, date);

            var price = GetPricedDto(fundId, date);

            if (price == null) return CreateNoPriceResult(fundId, date);

            return new FundPrice()
            {
                FundId = fundId,
                Date = date,
                Price = price.Price
            };
        }

        private PriceDto GetPricedDto(
            Guid fundId,
            DateTime date)
        {
            var prices = _priceDataTableGateway.GetAll();

            return prices
                .SingleOrDefault(d => d.InvestmentVehicleId == fundId && d.CloseDate == date);
        }

        private InvestmentVehicleDto GetInvestmentVehicleDto(
            Guid fundId)
        {
            return _investmentVehicleDataTableGateway
                .GetAll()
                .SingleOrDefault(d => d.InvestmentVehicleId == fundId);
        }

        private static FundPrice CreateNoPriceResult(
            Guid fundId,
            DateTime date) =>
            new FundPrice()
            {
                FundId = fundId,
                Date = date
            };
    }
}