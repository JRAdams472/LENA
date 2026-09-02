CREATE PROCEDURE [Inventory].[usp_Item_GetBrands]
    @UserID INT,
    @Search NVARCHAR(100) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    IF (@Search IS NULL OR @Search = '')
        SELECT ib.[Name]
        FROM [Inventory].[ItemBrand] ib
        WHERE EXISTS (
            SELECT 1
            FROM [Inventory].[UserItem] ui
            JOIN [Inventory].[Item] i ON i.[ItemID] = ui.[ItemID]
            WHERE i.[BrandID] = ib.[ItemBrandID]
              AND ui.[UserID] = @UserID
              AND ui.[CurrentQuantity] > 0
        )
        ORDER BY ib.[Name];
    ELSE
        SELECT [Name]
        FROM [Inventory].[ItemBrand]
        WHERE [Name] LIKE '%' + @Search + '%'
        ORDER BY [Name];
END
