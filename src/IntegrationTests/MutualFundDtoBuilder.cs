using System;
using MutualFundPerformance.SharedKernel.Infrastructure.MutualFundData;

namespace MutualFundPerformance.IntegrationTests
{
    public class MutualFundDtoBuilder
    {
        private readonly MutualFundDto _mutualFundDto;

        public MutualFundDtoBuilder(
            string name)
        {
            _mutualFundDto = new MutualFundDto()
            {
                MutualFundId = Guid.NewGuid(),
                Name = name,
                Symbol = "1234"
            };
        }

        public MutualFundDtoBuilder(string name, Guid id)
        {
            _mutualFundDto = new MutualFundDto()
            {
                MutualFundId = id,
                Name = name,
                Symbol = "1234"
            };
        }

        public MutualFundDtoBuilder WithSymbol(string s)
        {
            _mutualFundDto.Symbol = s;

            return this;
        }


        public MutualFundDto Create()
            {
                return _mutualFundDto;
            }
        }
}