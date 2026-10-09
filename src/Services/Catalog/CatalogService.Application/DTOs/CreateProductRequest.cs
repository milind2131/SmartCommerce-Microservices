namespace CatalogService.Application.DTOs;

public sealed record CreateProductRequest(
    string Name,
    string? Description,
    decimal Price);
