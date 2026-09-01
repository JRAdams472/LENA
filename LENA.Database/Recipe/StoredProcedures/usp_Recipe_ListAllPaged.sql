CREATE PROCEDURE [Recipe].[usp_Recipe_ListAllPaged]
    @PageNumber INT = 1,
    @PageSize INT = 25
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT RecipeID, RecipeName, Description, Servings, PrepTimeMinutes, CookTimeMinutes, IsActive, CreatedBy, CreateDate, LastUpdatedBy, LastUpdatedDate
    FROM [Recipe].[Recipe]
    ORDER BY RecipeName
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

    SELECT COUNT(*) FROM [Recipe].[Recipe];
END
