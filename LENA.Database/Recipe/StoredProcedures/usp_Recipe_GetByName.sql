CREATE PROCEDURE [Recipe].[usp_Recipe_GetByName]
    @RecipeName NVARCHAR(200)
AS
BEGIN
    SELECT RecipeID, RecipeName, Description, Servings, PrepTimeMinutes, CookTimeMinutes, IsActive, CreatedBy, CreateDate, LastUpdatedBy, LastUpdatedDate
    FROM [Recipe].[Recipe]
    WHERE RecipeName = @RecipeName;
END
