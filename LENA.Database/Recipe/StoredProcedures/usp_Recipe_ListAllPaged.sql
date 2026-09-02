CREATE PROCEDURE [Recipe].[usp_Recipe_ListAllPaged]
    @PageNumber INT = 1,
    @PageSize INT = 25,
    @Search NVARCHAR(200) = NULL,
    @IsFavorite BIT = 0
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT RecipeID, RecipeName, Description, Servings, PrepTimeMinutes, CookTimeMinutes, IsActive, IsFavorite, CreatedBy, CreateDate, LastUpdatedBy, LastUpdatedDate
    FROM [Recipe].[Recipe] r
    WHERE (@Search IS NULL OR @Search = '' OR r.RecipeName LIKE '%' + @Search + '%'
           OR EXISTS (
               SELECT 1
               FROM [Recipe].[RecipeItem] ri
               JOIN [Inventory].[Item] i ON i.ItemID = ri.ItemID
               WHERE ri.RecipeID = r.RecipeID
                 AND i.Name LIKE '%' + @Search + '%'
           ))
      AND (@IsFavorite = 0 OR r.IsFavorite = 1)
    ORDER BY r.RecipeName
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

    SELECT COUNT(*)
    FROM [Recipe].[Recipe] r
    WHERE (@Search IS NULL OR @Search = '' OR r.RecipeName LIKE '%' + @Search + '%'
           OR EXISTS (
               SELECT 1
               FROM [Recipe].[RecipeItem] ri
               JOIN [Inventory].[Item] i ON i.ItemID = ri.ItemID
               WHERE ri.RecipeID = r.RecipeID
                 AND i.Name LIKE '%' + @Search + '%'
           ))
      AND (@IsFavorite = 0 OR r.IsFavorite = 1);
END
