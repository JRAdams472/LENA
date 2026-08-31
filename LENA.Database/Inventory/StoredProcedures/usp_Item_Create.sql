CREATE PROCEDURE [Inventory].[usp_Item_Create]
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
    INSERT INTO [Inventory].[Item]
        ([Name], [Brand], [UPC12], [UPC14], [CategoryID], [Unit], [CurrentQuantity], [MinQuantity],
         [PurchaseDate], [ExpiryDate], [Notes], [IsFavorite], [CreatedBy], [CreateDate])
    VALUES
        (@Name, @Brand, @UPC12, @UPC14, @CategoryID, @Unit, @CurrentQuantity, @MinQuantity,
         @PurchaseDate, @ExpiryDate, @Notes, @IsFavorite, @CreatedBy, @CreateDate);
    SELECT CAST(SCOPE_IDENTITY() as int);
END