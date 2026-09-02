CREATE PROCEDURE [Inventory].[usp_Item_ListAll]
    @UserID INT,
    @PageNumber INT = 1,
    @PageSize INT = 25
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

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
    ORDER BY i.[Name]
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

    SELECT COUNT(*) FROM [Inventory].[Item];
END