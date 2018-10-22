using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MutualFundPerformance.Database.MutualFund;
using MutualFundPerformance.SharedKernel;

namespace MutualFundPerformance.WebApi.Controllers
{
    public class MutualFundPrice : Controller
    {
        private readonly MutualFundDataTableGateway _mutualFundDataTableGateway;

        public MutualFundPrice(
            IMutualFundPerformanceDatabaseSettings mutualFundPerformanceDatabaseSettings)
        {
            _mutualFundDataTableGateway = new MutualFundDataTableGateway(
                mutualFundPerformanceDatabaseSettings);
        }

        public string[] GetAllFunds()
        {
            var mutualFundDtos = _mutualFundDataTableGateway.GetAll();

            var fundNames = mutualFundDtos
                .Select(d => d.Name)
                .OrderBy(n => n)
                .ToArray();

            return fundNames;
        }
    }
}