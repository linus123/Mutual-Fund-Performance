using System;
using System.Data.SqlClient;
using MutualFundPerformance.SharedKernel;

namespace MutualFundPerformance.Database
{
    public abstract class BaseMutualFundPerformanceDataTableGateway
    {
        private readonly IMutualFundPerformanceDatabaseSettings _mutualFundPerformanceDatabaseSettings;

        protected BaseMutualFundPerformanceDataTableGateway(
            IMutualFundPerformanceDatabaseSettings mutualFundPerformanceDatabaseSettings)
        {
            _mutualFundPerformanceDatabaseSettings = mutualFundPerformanceDatabaseSettings;

            var sqlDatabaseHelper = new SqlDatabaseHelper();
        }

        protected void ConnectionExecute(
            Action<SqlConnection> connectionAction,
            string logSql)
        {
            using (var sqlConnection = new SqlConnection(_mutualFundPerformanceDatabaseSettings.MutualFundPerformanceDatabaseConnectionString))
            {
                sqlConnection.Open();
                connectionAction(sqlConnection);
                sqlConnection.Close();
            }
        }
    }
}