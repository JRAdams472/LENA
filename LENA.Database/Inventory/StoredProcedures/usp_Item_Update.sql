CREATE PROCEDURE [Inventory].[usp_Item_Update]
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
    UPDATE [Inventory].[Item]
    SET [Name] = @Name, [Brand] = @Brand, [UPC12] = @UPC12, [UPC14] = @UPC14,
        [CategoryID] = @CategoryID, [Unit] = @Unit, [CurrentQuantity] = @CurrentQuantity,
        [MinQuantity] = @MinQuantity, [PurchaseDate] = @PurchaseDate, [ExpiryDate] = @ExpiryDate,
        [Notes] = @Notes, [IsFavorite] = @IsFavorite,
        [LastUpdatedBy] = @LastUpdatedBy, [LastUpdatedDate] = @LastUpdatedDate
    WHERE [ItemID] = @ItemID;
END