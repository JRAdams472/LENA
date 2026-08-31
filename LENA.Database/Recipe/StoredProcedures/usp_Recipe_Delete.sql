CREATE PROCEDURE [Recipe].[usp_Recipe_Delete]
    @RecipeID INT
AS
BEGIN
    DELETE FROM [Recipe].[Recipe] WHERE RecipeID = @RecipeID;
END
