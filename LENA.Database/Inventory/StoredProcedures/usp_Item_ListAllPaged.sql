CREATE OR ALTER PROCEDURE [Inventory].[usp_Item_ListAllPaged]
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

    SELECT i.*, b.[Name] AS [Brand]
    FROM [Inventory].[Item] i
    LEFT JOIN [Inventory].[ItemBrand] b ON b.[ItemBrandID] = i.[BrandID]
    WHERE (@Search IS NULL OR @Search = '' OR i.[Name] LIKE '%' + @Search + '%')
      AND (@Brand IS NULL OR @Brand = '' OR b.[Name] = @Brand)
      AND (@InStock = 0 OR i.[CurrentQuantity] > 0)
      AND (@IsFavorite = 0 OR i.[IsFavorite] = 1)
    ORDER BY i.[Name]
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

    SELECT COUNT(*)
    FROM [Inventory].[Item] i
    LEFT JOIN [Inventory].[ItemBrand] b ON b.[ItemBrandID] = i.[BrandID]
    WHERE (@Search IS NULL OR @Search = '' OR i.[Name] LIKE '%' + @Search + '%')
      AND (@Brand IS NULL OR @Brand = '' OR b.[Name] = @Brand)
      AND (@InStock = 0 OR i.[CurrentQuantity] > 0)
      AND (@IsFavorite = 0 OR i.[IsFavorite] = 1);
END