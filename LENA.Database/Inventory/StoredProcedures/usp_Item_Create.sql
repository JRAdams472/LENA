CREATE OR ALTER PROCEDURE [Inventory].[usp_Item_Create]
    @Name NVARCHAR(200),
    @Brand NVARCHAR(100) = NULL,
    @UPC12 NVARCHAR(12) = NULL,
    @UPC14 NVARCHAR(14) = NULL,
    @CategoryID INT,
    @Unit NVARCHAR(20),
    @CurrentQuantity DECIMAL(10, 2),
    @MinQuantity DECIMAL(10, 2) = NULL,
    @PurchaseDate DATETIME2,
    @ExpiryDate DATETIME2 = NULL,
    @Notes NVARCHAR(500) = NULL,
    @IsFavorite BIT = 0,
    @CreatedBy NVARCHAR(100),
    @CreateDate DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @BrandID INT = NULL;

    IF @Brand IS NOT NULL AND @Brand <> ''
    BEGIN
        IF NOT EXISTS (SELECT 1 FROM [Inventory].[ItemBrand] WHERE [Name] = @Brand)
            INSERT INTO [Inventory].[ItemBrand] ([Name]) VALUES (@Brand);

        SELECT @BrandID = [ItemBrandID]
        FROM [Inventory].[ItemBrand]
        WHERE [Name] = @Brand;
    END

    INSERT INTO [Inventory].[Item]
        ([Name], [BrandID], [UPC12], [UPC14], [CategoryID], [Unit], [CurrentQuantity], [MinQuantity],
         [PurchaseDate], [ExpiryDate], [Notes], [IsFavorite], [CreatedBy], [CreateDate])
    VALUES
        (@Name, @BrandID, @UPC12, @UPC14, @CategoryID, @Unit, @CurrentQuantity, @MinQuantity,
         @PurchaseDate, @ExpiryDate, @Notes, @IsFavorite, @CreatedBy, @CreateDate);
    SELECT CAST(SCOPE_IDENTITY() as int);
END