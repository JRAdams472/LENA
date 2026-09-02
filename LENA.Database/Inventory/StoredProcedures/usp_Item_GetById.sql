CREATE PROCEDURE [Inventory].[usp_Item_GetById]
    @Id INT,
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT i.[ItemID], i.[Name], i.[BrandID], i.[UPC12], i.[UPC14], i.[CategoryID], i.[Unit], b.[Name] AS [Brand],
           COALESCE(ui.[CurrentQuantity], 0) AS [CurrentQuantity],
           ui.[MinQuantity],
           ui.[PurchaseDate],
           ui.[ExpiryDate],
           ui.[Notes],
           COALESCE(ui.[IsFavorite], 0) AS [IsFavorite],
           i.[CreatedBy], i.[CreateDate], i.[LastUpdatedBy], i.[LastUpdatedDate]
    FROM [Inventory].[Item] i
    LEFT JOIN [Inventory].[ItemBrand] b ON b.[ItemBrandID] = i.[BrandID]
    LEFT JOIN [Inventory].[UserItem] ui ON ui.[ItemID] = i.[ItemID] AND ui.[UserID] = @UserID
    WHERE i.[ItemID] = @Id;
END