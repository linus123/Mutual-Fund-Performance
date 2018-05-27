using System.Linq;
using Dapper;
using MutualFundPerformance.SharedKernel;
using MutualFundPerformance.SharedKernel.Infrastructure.MutualFundData;

namespace MutualFundPerformance.Database.MutualFund
{
    public class BenchmarkDataTableGateway : BaseMutualFundPerformanceDataTableGateway
    {
        public BenchmarkDataTableGateway(
            IMutualFundPerformanceDatabaseSettings mutualFundPerformanceDatabaseSettings)
            : base(mutualFundPerformanceDatabaseSettings)
        {
        }

        public BenchmarkDto[] GetAll()
        {
            const string sql = @"
SELECT
        [BenchmarkId]
        ,[Name]
        ,[MutualFundId]
    FROM
        [MutualFund].[Benchmark]";

            BenchmarkDto[] result = null;

            ConnectionExecute(
                connection =>
                {
                    result = connection.Query<BenchmarkDto>(sql).ToArray();
                },
                sql);

            return result;
        }

        public void Insert(BenchmarkDto[] dtos)
        {
            const string sql = @"
INSERT INTO
        [MutualFund].[Benchmark]
        ([BenchmarkId], [Name], [MutualFundId])
    VALUES
        (@BenchmarkId, @Name, @MutualFundId)";

            BenchmarkDto[] result = null;

            ConnectionExecute(connection => connection.Execute(sql, dtos), sql);
        }

        public void DeleteAll()
        {
            const string sql = @"DELETE FROM [MutualFund].[Benchmark]";

            ConnectionExecute(connection => connection.Execute(sql), sql);
        }
    }
}