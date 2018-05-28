using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using MutualFundPerformance.Database.HistoricalPriceData;
using MutualFundPerformance.Database.MutualFund;

namespace MutualFundPerformance.WebApi.Controllers
{

    [Route("api/[controller]")]
    public class PriceController : Controller
    {
        private MutualFundDataTableGateway _mutualFundDataTableGateway;
        private InvestmentVehicleDataTableGateway _investmentVehicleDataTableGateway;
        private PriceDataTableGateway _priceDataTableGateway;

        public PriceController(
            MutualFundDataTableGateway mutualFundDataTableGateway,
            InvestmentVehicleDataTableGateway investmentVehicleDataTableGateway,
            PriceDataTableGateway priceDataTableGateway)
        {
            _priceDataTableGateway = priceDataTableGateway;
            _investmentVehicleDataTableGateway = investmentVehicleDataTableGateway;
            _mutualFundDataTableGateway = mutualFundDataTableGateway;
        }

        [HttpPost("GetAll")]
        public object[] GetAll(
            string[] symbols)
        {
            var mutualFundDtos = _mutualFundDataTableGateway.GetAll();

            var mutualFundDto = mutualFundDtos.FirstOrDefault(d => d.Symbol == symbols[0]);

            if (mutualFundDto == null)
                return new object[0];

            var investmentVehicleDtos = _investmentVehicleDataTableGateway.GetAll();

            var investmentVehicleDto = investmentVehicleDtos.FirstOrDefault(d => d.ExternalId == mutualFundDto.MutualFundId);

            if (investmentVehicleDto == null)
                return new object[0];

            var priceDtos = _priceDataTableGateway.GetAll();

            var priceDto = priceDtos.FirstOrDefault(d => d.InvestmentVehicleId == investmentVehicleDto.InvestmentVehicleId);

            if (priceDto == null)
                return new object[0];

            return new object[1];
        }

    }
}
