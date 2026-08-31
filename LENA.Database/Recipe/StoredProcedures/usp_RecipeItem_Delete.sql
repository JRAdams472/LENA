CREATE PROCEDURE [Recipe].[usp_RecipeItem_Delete]
    @RecipeID INT,
    @ItemID INT
AS
BEGIN
    DELETE FROM [Recipe].[RecipeItem] WHERE RecipeID = @RecipeID AND ItemID = @ItemID;
END
