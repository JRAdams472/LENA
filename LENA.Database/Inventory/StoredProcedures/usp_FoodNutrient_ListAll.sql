CREATE PROCEDURE [Inventory].[usp_FoodNutrient_ListAll]
    @PageNumber INT = 1,
    @PageSize INT = 25
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT food_id AS FoodId,
           nutrient_id AS NutrientId,
           amount_per_serving AS AmountPerServing
    FROM [Inventory].[food_nutrients]
    ORDER BY food_id, nutrient_id
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

    SELECT COUNT(*) FROM [Inventory].[food_nutrients];
END