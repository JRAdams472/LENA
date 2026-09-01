CREATE OR ALTER PROCEDURE [Inventory].[usp_Item_GetBrands]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [Name]
    FROM [Inventory].[ItemBrand]
    ORDER BY [Name];
END
