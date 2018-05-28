using System.Linq;
using Dapper;
using MutualFundPerformance.SharedKernel;
using MutualFundPerformance.SharedKernel.Infrastructure.MutualFundData;

namespace MutualFundPerformance.Database.MutualFund
{
    public class MutualFundDataTableGateway : BaseMutualFundPerformanceDataTableGateway
    {
        public MutualFundDataTableGateway(
            IMutualFundPerformanceDatabaseSettings mutualFundPerformanceDatabaseSettings)
            : base(mutualFundPerformanceDatabaseSettings)
        {
        }

        public MutualFundDto[] GetAll()
        {
            const string sql = @"
SELECT
        [MutualFundId]
        ,[Name]
    FROM
        [MutualFund].[MutualFund]";

            MutualFundDto[] result = null;

            ConnectionExecute(
                connection =>
                {
                    result = connection.Query<MutualFundDto>(sql).ToArray();
                },
                sql);

            return result;
        }

        public void Insert(MutualFundDto[] dtos)
        {
            const string sql = @"
INSERT INTO
        [MutualFund].[MutualFund]
        ([MutualFundId], [Name])
    VALUES
        (@MutualFundId, @Name)";

            ConnectionExecute(connection => connection.Execute(sql, dtos), sql);
        }

        public void DeleteAll()
        {
            const string sql = @"DELETE FROM [MutualFund].[MutualFund]";

            ConnectionExecute(connection => connection.Execute(sql), sql);
        }
    }
}