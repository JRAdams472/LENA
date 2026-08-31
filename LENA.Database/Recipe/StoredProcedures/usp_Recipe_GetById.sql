CREATE PROCEDURE [Recipe].[usp_Recipe_GetById]
    @RecipeID INT
AS
BEGIN
    SELECT RecipeID, RecipeName, Description, Servings, PrepTimeMinutes, CookTimeMinutes, IsActive, CreatedBy, CreateDate, LastUpdatedBy, LastUpdatedDate
    FROM [Recipe].[Recipe]
    WHERE RecipeID = @RecipeID;
END
