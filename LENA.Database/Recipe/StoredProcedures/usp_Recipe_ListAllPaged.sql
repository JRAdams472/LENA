CREATE PROCEDURE [Recipe].[usp_Recipe_ListAllPaged]
    @UserID INT,
    @PageNumber INT = 1,
    @PageSize INT = 25,
    @Search NVARCHAR(200) = NULL,
    @IsFavorite BIT = 0
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT r.RecipeID, r.RecipeName, r.Description, r.Servings, r.PrepTimeMinutes, r.CookTimeMinutes, r.IsActive,
           COALESCE(urp.IsFavorite, 0) AS IsFavorite,
           r.CreatedBy, r.CreateDate, r.LastUpdatedBy, r.LastUpdatedDate
    FROM [Recipe].[Recipe] r
    LEFT JOIN [Recipe].[UserRecipePreference] urp ON r.RecipeID = urp.RecipeID AND urp.UserID = @UserID
    WHERE (@Search IS NULL OR @Search = '' OR r.RecipeName LIKE '%' + @Search + '%'
           OR EXISTS (
               SELECT 1
               FROM [Recipe].[RecipeItem] ri
               JOIN [Inventory].[Item] i ON i.ItemID = ri.ItemID
               WHERE ri.RecipeID = r.RecipeID
                 AND i.Name LIKE '%' + @Search + '%'
           ))
      AND (@IsFavorite = 0 OR COALESCE(urp.IsFavorite, 0) = 1)
    ORDER BY r.RecipeName
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

    SELECT COUNT(*)
    FROM [Recipe].[Recipe] r
    LEFT JOIN [Recipe].[UserRecipePreference] urp ON r.RecipeID = urp.RecipeID AND urp.UserID = @UserID
    WHERE (@Search IS NULL OR @Search = '' OR r.RecipeName LIKE '%' + @Search + '%'
           OR EXISTS (
               SELECT 1
               FROM [Recipe].[RecipeItem] ri
               JOIN [Inventory].[Item] i ON i.ItemID = ri.ItemID
               WHERE ri.RecipeID = r.RecipeID
                 AND i.Name LIKE '%' + @Search + '%'
           ))
      AND (@IsFavorite = 0 OR COALESCE(urp.IsFavorite, 0) = 1);
END
