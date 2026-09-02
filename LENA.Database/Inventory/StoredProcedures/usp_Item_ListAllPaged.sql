CREATE PROCEDURE [Inventory].[usp_Item_ListAllPaged]
    @UserID INT,
    @PageNumber INT = 1,
    @PageSize INT = 25,
    @Search NVARCHAR(200) = NULL,
    @Brand NVARCHAR(100) = NULL,
    @InStock BIT = 0,
    @IsFavorite BIT = 0
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
    WHERE (@Search IS NULL OR @Search = '' OR i.[Name] LIKE '%' + @Search + '%')
      AND (@Brand IS NULL OR @Brand = '' OR b.[Name] = @Brand)
      AND (@InStock = 0 OR COALESCE(ui.[CurrentQuantity], 0) > 0)
      AND (@IsFavorite = 0 OR COALESCE(ui.[IsFavorite], 0) = 1)
    ORDER BY i.[Name]
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

    SELECT COUNT(*)
    FROM [Inventory].[Item] i
    LEFT JOIN [Inventory].[ItemBrand] b ON b.[ItemBrandID] = i.[BrandID]
    LEFT JOIN [Inventory].[UserItem] ui ON ui.[ItemID] = i.[ItemID] AND ui.[UserID] = @UserID
    WHERE (@Search IS NULL OR @Search = '' OR i.[Name] LIKE '%' + @Search + '%')
      AND (@Brand IS NULL OR @Brand = '' OR b.[Name] = @Brand)
      AND (@InStock = 0 OR COALESCE(ui.[CurrentQuantity], 0) > 0)
      AND (@IsFavorite = 0 OR COALESCE(ui.[IsFavorite], 0) = 1);
END