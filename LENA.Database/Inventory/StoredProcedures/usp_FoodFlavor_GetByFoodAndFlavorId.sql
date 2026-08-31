CREATE PROCEDURE [Inventory].[usp_FoodFlavor_GetByFoodAndFlavorId]
    @FoodId INT,
    @FlavorId INT
AS
BEGIN
    SELECT food_id AS FoodId,
           flavor_id AS FlavorId,
           intensity_score AS IntensityScore
    FROM [Inventory].[food_flavors]
    WHERE food_id = @FoodId AND flavor_id = @FlavorId;
END