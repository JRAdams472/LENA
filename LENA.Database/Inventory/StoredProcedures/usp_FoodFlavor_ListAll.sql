CREATE PROCEDURE [Inventory].[usp_FoodFlavor_ListAll]
    @PageNumber INT = 1,
    @PageSize INT = 25
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT food_id AS FoodId,
           flavor_id AS FlavorId,
           intensity_score AS IntensityScore
    FROM [Inventory].[food_flavors]
    ORDER BY food_id, flavor_id
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

    SELECT COUNT(*) FROM [Inventory].[food_flavors];
END