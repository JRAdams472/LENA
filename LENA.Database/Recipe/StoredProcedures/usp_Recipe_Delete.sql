CREATE PROCEDURE [Recipe].[usp_Recipe_Delete]
    @RecipeID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [Recipe].[Recipe] WHERE RecipeID = @RecipeID;

    SELECT @@ROWCOUNT;
END
