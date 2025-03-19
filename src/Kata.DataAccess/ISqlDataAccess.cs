using Microsoft.Data.SqlClient;
using System.Data;

namespace Kata.DataAccess
{
    public interface ISqlDataAccess
    {
        IDbConnection CreateConnection();
    }
}