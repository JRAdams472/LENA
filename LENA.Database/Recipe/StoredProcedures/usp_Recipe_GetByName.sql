CREATE PROCEDURE [Recipe].[usp_Recipe_GetByName]
    @RecipeName NVARCHAR(200),
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT r.RecipeID, r.RecipeName, r.Description, r.Servings, r.PrepTimeMinutes, r.CookTimeMinutes, r.IsActive,
           COALESCE(urp.IsFavorite, 0) AS IsFavorite,
           r.CreatedBy, r.CreateDate, r.LastUpdatedBy, r.LastUpdatedDate
    FROM [Recipe].[Recipe] r
    LEFT JOIN [Recipe].[UserRecipePreference] urp ON r.RecipeID = urp.RecipeID AND urp.UserID = @UserID
    WHERE r.RecipeName = @RecipeName;
END
