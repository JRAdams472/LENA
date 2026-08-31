CREATE PROCEDURE [Recipe].[usp_RecipeItem_GetByRecipeId]
    @RecipeID INT
AS
BEGIN
    SELECT RecipeID, ItemID, Quantity, UnitOfMeasure, Notes, IsOptional
    FROM [Recipe].[RecipeItem]
    WHERE RecipeID = @RecipeID;
END
