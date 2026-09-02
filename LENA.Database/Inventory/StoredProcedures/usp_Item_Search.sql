CREATE PROCEDURE [Inventory].[usp_Item_Search]
    @UserID INT,
    @Search NVARCHAR(200),
    @Brand NVARCHAR(100) = NULL,
    @Limit INT = 50
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (@Limit) i.[ItemID], i.[Name], i.[BrandID], i.[UPC12], i.[UPC14], i.[CategoryID], i.[Unit], b.[Name] AS [Brand],
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
    WHERE (i.[Name] LIKE '%' + @Search + '%' OR b.[Name] LIKE '%' + @Search + '%')
      AND (@Brand IS NULL OR @Brand = '' OR b.[Name] = @Brand)
    ORDER BY i.[Name];
END
