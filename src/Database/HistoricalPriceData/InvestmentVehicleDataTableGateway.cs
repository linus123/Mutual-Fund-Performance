using System.Linq;
using Dapper;
using MutualFundPerformance.SharedKernel;
using MutualFundPerformance.SharedKernel.Infrastructure.HistoricalPriceData;

namespace MutualFundPerformance.Database.HistoricalPriceData
{
    public class InvestmentVehicleDataTableGateway : BaseMutualFundPerformanceDataTableGateway
    {
        public InvestmentVehicleDataTableGateway(
            IMutualFundPerformanceDatabaseSettings mutualFundPerformanceDatabaseSettings)
            : base(mutualFundPerformanceDatabaseSettings)
        {
        }

        public InvestmentVehicleDto[] GetAll()
        {
            const string sql = @"
SELECT
        [InvestmentVehicleId]
        ,[Name]
        ,[ExternalId]
    FROM
        [HistoricalPrice].[InvestmentVehicle]";

            InvestmentVehicleDto[] result = null;

            ConnectionExecute(
                connection =>
                {
                    result = connection.Query<InvestmentVehicleDto>(sql).ToArray();
                },
                sql);

            return result;
        }

        public void Insert(InvestmentVehicleDto[] dtos)
        {
            const string sql = @"
INSERT INTO
        [HistoricalPrice].[InvestmentVehicle]
        ([InvestmentVehicleId], [Name], [ExternalId])
    VALUES
        (@InvestmentVehicleId, @Name, @ExternalId)";

            InvestmentVehicleDto[] result = null;

            ConnectionExecute(connection => connection.Execute(sql, dtos), sql);
        }

        public void DeleteAll()
        {
            const string sql = @"DELETE FROM [HistoricalPrice].[InvestmentVehicle]";

            ConnectionExecute(connection => connection.Execute(sql), sql);
        }
    }
}