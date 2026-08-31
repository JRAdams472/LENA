CREATE PROCEDURE [Recipe].[usp_RecipeItem_ListAll]
AS
BEGIN
    SELECT RecipeID, ItemID, Quantity, UnitOfMeasure, Notes
    FROM [Recipe].[RecipeItem];
END
