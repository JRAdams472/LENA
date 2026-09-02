CREATE PROCEDURE [Inventory].[usp_Item_Create]
    @Name NVARCHAR(200),
    @Brand NVARCHAR(100) = NULL,
    @UPC12 NVARCHAR(12) = NULL,
    @UPC14 NVARCHAR(14) = NULL,
    @CategoryID INT,
    @Unit NVARCHAR(20),
    @UserID INT,
    @CurrentQuantity DECIMAL(10, 2) = 0,
    @MinQuantity DECIMAL(10, 2) = NULL,
    @PurchaseDate DATETIME2 = NULL,
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
        ([Name], [BrandID], [UPC12], [UPC14], [CategoryID], [Unit], [CreatedBy], [CreateDate])
    VALUES
        (@Name, @BrandID, @UPC12, @UPC14, @CategoryID, @Unit, @CreatedBy, @CreateDate);

    DECLARE @ItemID INT = CAST(SCOPE_IDENTITY() AS INT);

    INSERT INTO [Inventory].[UserItem]
        ([UserID], [ItemID], [CurrentQuantity], [MinQuantity], [PurchaseDate], [ExpiryDate], [Notes], [IsFavorite], [CreatedBy], [CreateDate])
    VALUES
        (@UserID, @ItemID, @CurrentQuantity, @MinQuantity, @PurchaseDate, @ExpiryDate, @Notes, @IsFavorite, @CreatedBy, @CreateDate);

    SELECT @ItemID;
END