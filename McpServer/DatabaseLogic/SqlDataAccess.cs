using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;

namespace McpServer.DatabaseLogic;

public class SqlDataAccess
{

    public List<T> LoadData<T, U>(string sqlStatement, U parameters, string connectionString)
    {
        using IDbConnection connection = new SqlConnection(connectionString);

        return connection
              .Query<T>(sqlStatement, (object)parameters)
              .ToList();
    }

    //public void SaveData<T>(string sqlStatement, T parameters, string connectionString)
    //{
    //    using (IDbConnection connection = new SqlConnection(connectionString))
    //    {
    //        connection.Execute(sqlStatement, parameters);
    //    }
    //}
}
