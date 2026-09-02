CREATE PROCEDURE [Recipe].[usp_Recipe_ListAll]
    @UserID INT,
    @PageNumber INT = 1,
    @PageSize INT = 25
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT r.RecipeID, r.RecipeName, r.Description, r.Servings, r.PrepTimeMinutes, r.CookTimeMinutes, r.IsActive,
           COALESCE(urp.IsFavorite, 0) AS IsFavorite,
           r.CreatedBy, r.CreateDate, r.LastUpdatedBy, r.LastUpdatedDate
    FROM [Recipe].[Recipe] r
    LEFT JOIN [Recipe].[UserRecipePreference] urp ON r.RecipeID = urp.RecipeID AND urp.UserID = @UserID
    ORDER BY r.RecipeName
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

    SELECT COUNT(*) FROM [Recipe].[Recipe];
END
