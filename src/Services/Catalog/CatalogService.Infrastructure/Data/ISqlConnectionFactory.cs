using System.Data.Common;

namespace CatalogService.Infrastructure.Data;

public interface ISqlConnectionFactory
{
    DbConnection CreateConnection();
}
