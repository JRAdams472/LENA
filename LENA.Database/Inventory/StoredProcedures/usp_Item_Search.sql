CREATE PROCEDURE [Inventory].[usp_Item_Search]
    @Search NVARCHAR(200),
    @Brand NVARCHAR(100) = NULL,
    @Limit INT = 50
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (@Limit) i.*, b.[Name] AS [Brand]
    FROM [Inventory].[Item] i
    LEFT JOIN [Inventory].[ItemBrand] b ON b.[ItemBrandID] = i.[BrandID]
    WHERE (i.[Name] LIKE '%' + @Search + '%' OR b.[Name] LIKE '%' + @Search + '%')
      AND (@Brand IS NULL OR @Brand = '' OR b.[Name] = @Brand)
    ORDER BY i.[Name];
END
