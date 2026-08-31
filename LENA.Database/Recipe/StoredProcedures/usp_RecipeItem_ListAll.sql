CREATE PROCEDURE [Recipe].[usp_RecipeItem_ListAll]
AS
BEGIN
    SELECT RecipeID, ItemID, Quantity, UnitOfMeasure, Notes, IsOptional
    FROM [Recipe].[RecipeItem];
END
