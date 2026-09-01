CREATE OR ALTER PROCEDURE [Inventory].[usp_Item_Update]
    @ItemID INT,
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
    @LastUpdatedBy NVARCHAR(100) = NULL,
    @LastUpdatedDate DATETIME2 = NULL
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

    UPDATE [Inventory].[Item]
    SET [Name] = @Name, [BrandID] = @BrandID, [UPC12] = @UPC12, [UPC14] = @UPC14,
        [CategoryID] = @CategoryID, [Unit] = @Unit, [CurrentQuantity] = @CurrentQuantity,
        [MinQuantity] = @MinQuantity, [PurchaseDate] = @PurchaseDate, [ExpiryDate] = @ExpiryDate,
        [Notes] = @Notes, [IsFavorite] = @IsFavorite,
        [LastUpdatedBy] = @LastUpdatedBy, [LastUpdatedDate] = @LastUpdatedDate
    WHERE [ItemID] = @ItemID;

    SELECT @@ROWCOUNT;
END
