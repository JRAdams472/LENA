CREATE PROCEDURE [Inventory].[usp_FoodFlavor_GetByFoodId]
    @FoodId INT
AS
BEGIN
    SELECT food_id AS FoodId,
           flavor_id AS FlavorId,
           intensity_score AS IntensityScore
    FROM [Inventory].[food_flavors]
    WHERE food_id = @FoodId;
END