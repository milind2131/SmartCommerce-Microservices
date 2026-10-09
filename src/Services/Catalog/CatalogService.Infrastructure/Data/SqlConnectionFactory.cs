using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace CatalogService.Infrastructure.Data;

internal sealed class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
{
    public DbConnection CreateConnection()
    {
        return new SqlConnection(connectionString);
    }
}
