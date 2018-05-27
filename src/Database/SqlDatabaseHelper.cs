using System;
using System.Data.SqlClient;

namespace MutualFundPerformance.Database
{
    public sealed class SqlDatabaseHelper
    {
        public void ConnectionExecute(
            string connectionString,
            Action<SqlConnection> connectionAction,
            string logSql)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                connectionAction(sqlConnection);
                sqlConnection.Close();
            }
        }
    }
}