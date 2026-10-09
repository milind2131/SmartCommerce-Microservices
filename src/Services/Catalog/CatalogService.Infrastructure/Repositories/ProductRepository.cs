using CatalogService.Application.Abstractions;
using CatalogService.Domain.Entities;
using CatalogService.Infrastructure.Data;
using Dapper;

namespace CatalogService.Infrastructure.Repositories;

internal sealed class ProductRepository(ISqlConnectionFactory connectionFactory) : IProductRepository
{
    public async Task<IReadOnlyCollection<Product>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT ProductId, Name, Description, Price, IsActive, CreatedAtUtc, UpdatedAtUtc
            FROM dbo.Products
            WHERE IsActive = 1
            ORDER BY CreatedAtUtc DESC;
            """;

        await using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition(sql, cancellationToken: cancellationToken);
        var products = await connection.QueryAsync<Product>(command);

        return products.ToArray();
    }

    public async Task<Product?> GetByIdAsync(
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT ProductId, Name, Description, Price, IsActive, CreatedAtUtc, UpdatedAtUtc
            FROM dbo.Products
            WHERE ProductId = @ProductId AND IsActive = 1;
            """;

        await using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition(
            sql,
            new { ProductId = productId },
            cancellationToken: cancellationToken);

        return await connection.QuerySingleOrDefaultAsync<Product>(command);
    }

    public async Task AddAsync(
        Product product,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            INSERT INTO dbo.Products
            (
                ProductId,
                Name,
                Description,
                Price,
                IsActive,
                CreatedAtUtc,
                UpdatedAtUtc
            )
            VALUES
            (
                @ProductId,
                @Name,
                @Description,
                @Price,
                @IsActive,
                @CreatedAtUtc,
                @UpdatedAtUtc
            );
            """;

        await using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition(
            sql,
            product,
            cancellationToken: cancellationToken);

        await connection.ExecuteAsync(command);
    }

    public async Task<bool> UpdateAsync(
        Product product,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE dbo.Products
            SET Name = @Name,
                Description = @Description,
                Price = @Price,
                UpdatedAtUtc = @UpdatedAtUtc
            WHERE ProductId = @ProductId AND IsActive = 1;
            """;

        await using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition(
            sql,
            product,
            cancellationToken: cancellationToken);

        return await connection.ExecuteAsync(command) > 0;
    }

    public async Task<bool> DeleteAsync(
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE dbo.Products
            SET IsActive = 0,
                UpdatedAtUtc = SYSUTCDATETIME()
            WHERE ProductId = @ProductId AND IsActive = 1;
            """;

        await using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition(
            sql,
            new { ProductId = productId },
            cancellationToken: cancellationToken);

        return await connection.ExecuteAsync(command) > 0;
    }
}
