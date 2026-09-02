CREATE PROCEDURE [Recipe].[usp_RecipeItem_GetByRecipeId]
    @RecipeID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT
        ri.RecipeID,
        ri.ItemID,
        ri.Quantity,
        ri.UnitOfMeasure,
        ri.Notes,
        ri.IsOptional,
        i.[Name] AS ItemName,
        ib.[Name] AS ItemBrand
    FROM [Recipe].[RecipeItem] ri
    LEFT JOIN [Inventory].[Item] i ON i.[ItemID] = ri.[ItemID]
    LEFT JOIN [Inventory].[ItemBrand] ib ON ib.[ItemBrandID] = i.[BrandID]
    WHERE ri.[RecipeID] = @RecipeID;
END
