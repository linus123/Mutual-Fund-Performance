using System;
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

        public FundListModel[] GetAllFunds()
        {
            var mutualFundDtos = _mutualFundDataTableGateway.GetAll();

            var fundNames = mutualFundDtos
                .Select(f => new FundListModel
                {
                    Id = f.MutualFundId,
                    Name = f.Name
                })
                .OrderBy(m => m.Name)
                .ToArray();

            return fundNames;
        }

        public class FundListModel
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }
}