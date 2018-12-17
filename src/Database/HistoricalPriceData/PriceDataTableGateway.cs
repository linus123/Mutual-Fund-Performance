using System.Linq;
using Dapper;
using MutualFundPerformance.SharedKernel;
using MutualFundPerformance.SharedKernel.Infrastructure.HistoricalPriceData;

namespace MutualFundPerformance.Database.HistoricalPriceData
{
    public class PriceDataTableGateway : BaseMutualFundPerformanceDataTableGateway, IPriceDataTableGateway
    {
        public PriceDataTableGateway(
            IMutualFundPerformanceDatabaseSettings mutualFundPerformanceDatabaseSettings)
            : base(mutualFundPerformanceDatabaseSettings)
        {
        }

        public PriceDto[] GetAll()
        {
            const string sql = @"
SELECT
        [InvestmentVehicleId]
        ,[CloseDate]
        ,[Price]
    FROM
        [HistoricalPrice].[Price]";

            PriceDto[] result = null;

            ConnectionExecute(
                connection =>
                {
                    result = connection.Query<PriceDto>(sql).ToArray();
                },
                sql);

            return result;
        }

        public void Insert(PriceDto[] dtos)
        {
            const string sql = @"
INSERT INTO
        [HistoricalPrice].[Price]
        ([InvestmentVehicleId], [CloseDate], [Price])
    VALUES
        (@InvestmentVehicleId, @CloseDate, @Price)";

            ConnectionExecute(connection => connection.Execute(sql, dtos), sql);
        }

        public void DeleteAll()
        {
            const string sql = @"DELETE FROM [HistoricalPrice].[Price]";

            ConnectionExecute(connection => connection.Execute(sql), sql);
        }
    }
}