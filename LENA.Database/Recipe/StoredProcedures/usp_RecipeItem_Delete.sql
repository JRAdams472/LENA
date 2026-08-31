CREATE PROCEDURE [Recipe].[usp_RecipeItem_Delete]
    @RecipeID INT,
    @ItemID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [Recipe].[RecipeItem] WHERE RecipeID = @RecipeID AND ItemID = @ItemID;

    SELECT @@ROWCOUNT;
END
