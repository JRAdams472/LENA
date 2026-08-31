CREATE PROCEDURE [Inventory].[usp_FoodNutrient_GetByFoodAndNutrientId]
    @FoodId INT,
    @NutrientId INT
AS
BEGIN
    SELECT food_id AS FoodId,
           nutrient_id AS NutrientId,
           amount_per_serving AS AmountPerServing
    FROM [Inventory].[food_nutrients]
    WHERE food_id = @FoodId AND nutrient_id = @NutrientId;
END