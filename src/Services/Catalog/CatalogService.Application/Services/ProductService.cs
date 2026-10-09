using CatalogService.Application.Abstractions;
using CatalogService.Application.DTOs;
using CatalogService.Domain.Entities;

namespace CatalogService.Application.Services;

internal sealed class ProductService(IProductRepository productRepository) : IProductService
{
    public async Task<IReadOnlyCollection<ProductResponse>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var products = await productRepository.GetAllAsync(cancellationToken);
        return products.Select(MapToResponse).ToArray();
    }

    public async Task<ProductResponse?> GetByIdAsync(
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetByIdAsync(productId, cancellationToken);
        return product is null ? null : MapToResponse(product);
    }

    public async Task<ProductResponse> CreateAsync(
        CreateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        Validate(request.Name, request.Price);

        var product = new Product
        {
            ProductId = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            Price = request.Price,
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow
        };

        await productRepository.AddAsync(product, cancellationToken);
        return MapToResponse(product);
    }

    public async Task<bool> UpdateAsync(
        Guid productId,
        UpdateProductRequest request,
        CancellationToken cancellationToken = default)
    {
        Validate(request.Name, request.Price);

        var existingProduct = await productRepository.GetByIdAsync(productId, cancellationToken);
        if (existingProduct is null)
        {
            return false;
        }

        existingProduct.Name = request.Name.Trim();
        existingProduct.Description = request.Description?.Trim();
        existingProduct.Price = request.Price;
        existingProduct.UpdatedAtUtc = DateTime.UtcNow;

        return await productRepository.UpdateAsync(existingProduct, cancellationToken);
    }

    public Task<bool> DeleteAsync(
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        return productRepository.DeleteAsync(productId, cancellationToken);
    }

    private static ProductResponse MapToResponse(Product product)
    {
        return new ProductResponse(
            product.ProductId,
            product.Name,
            product.Description,
            product.Price,
            product.CreatedAtUtc,
            product.UpdatedAtUtc);
    }

    private static void Validate(string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Product name is required.");
        }

        if (price <= 0)
        {
            throw new ArgumentException("Product price must be greater than zero.");
        }
    }
}
