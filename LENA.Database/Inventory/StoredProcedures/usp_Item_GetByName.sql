CREATE OR ALTER PROCEDURE [Inventory].[usp_Item_GetByName]
    @Name NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT i.*, b.[Name] AS [Brand]
    FROM [Inventory].[Item] i
    LEFT JOIN [Inventory].[ItemBrand] b ON b.[ItemBrandID] = i.[BrandID]
    WHERE i.[Name] = @Name;
END