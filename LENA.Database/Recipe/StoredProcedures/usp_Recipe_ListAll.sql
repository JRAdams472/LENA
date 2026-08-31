CREATE PROCEDURE [Recipe].[usp_Recipe_ListAll]
AS
BEGIN
    SELECT RecipeID, RecipeName, Description, Servings, PrepTimeMinutes, CookTimeMinutes, IsActive, CreatedBy, CreateDate, LastUpdatedBy, LastUpdatedDate
    FROM [Recipe].[Recipe];
END
