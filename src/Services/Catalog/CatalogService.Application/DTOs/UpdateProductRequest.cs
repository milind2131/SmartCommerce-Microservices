namespace CatalogService.Application.DTOs;

public sealed record UpdateProductRequest(
    string Name,
    string? Description,
    decimal Price);
