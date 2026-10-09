IF DB_ID(N'SmartCommerce_CatalogDb') IS NULL
BEGIN
    CREATE DATABASE SmartCommerce_CatalogDb;
END;
GO

USE SmartCommerce_CatalogDb;
GO

IF OBJECT_ID(N'dbo.Products', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Products
    (
        ProductId UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_Products PRIMARY KEY,
        Name NVARCHAR(200) NOT NULL,
        Description NVARCHAR(1000) NULL,
        Price DECIMAL(18, 2) NOT NULL,
        IsActive BIT NOT NULL CONSTRAINT DF_Products_IsActive DEFAULT (1),
        CreatedAtUtc DATETIME2 NOT NULL,
        UpdatedAtUtc DATETIME2 NULL,
        CONSTRAINT CK_Products_Price CHECK (Price > 0)
    );
END;
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Products)
BEGIN
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
    (NEWID(), N'Developer Laptop', N'Laptop suitable for .NET development.', 74999.00, 1, SYSUTCDATETIME(), NULL),
    (NEWID(), N'Mechanical Keyboard', N'Compact mechanical keyboard.', 4999.00, 1, SYSUTCDATETIME(), NULL),
    (NEWID(), N'USB-C Dock', N'USB-C dock for workstation connectivity.', 6999.00, 1, SYSUTCDATETIME(), NULL);
END;
GO
