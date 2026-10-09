namespace CatalogService.Application.DTOs;

public sealed record ProductResponse(
    Guid ProductId,
    string Name,
    string? Description,
    decimal Price,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc);
