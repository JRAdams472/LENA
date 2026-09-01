CREATE OR ALTER PROCEDURE [Inventory].[usp_Item_GetBrands]
    @Search NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF (@Search IS NULL OR @Search = '')
        SELECT ib.[Name]
        FROM [Inventory].[ItemBrand] ib
        WHERE EXISTS (
            SELECT 1
            FROM [Inventory].[Item] i
            WHERE i.[BrandID] = ib.[ItemBrandID]
              AND i.[CurrentQuantity] > 0
        )
        ORDER BY ib.[Name];
    ELSE
        SELECT [Name]
        FROM [Inventory].[ItemBrand]
        WHERE [Name] LIKE '%' + @Search + '%'
        ORDER BY [Name];
END
