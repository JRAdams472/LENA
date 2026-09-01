CREATE PROCEDURE [Inventory].[usp_NutrientType_ListAll]
    @PageNumber INT = 1,
    @PageSize INT = 25
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT nutrient_id AS NutrientId,
           nutrient_name AS NutrientName,
           unit_of_measure AS UnitOfMeasure
    FROM [Inventory].[nutrient_types]
    ORDER BY nutrient_name
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

    SELECT COUNT(*) FROM [Inventory].[nutrient_types];
END