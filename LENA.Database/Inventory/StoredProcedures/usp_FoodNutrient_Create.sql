CREATE PROCEDURE [Inventory].[usp_FoodNutrient_Create]
    @FoodId INT,
    @NutrientId INT,
    @AmountPerServing NUMERIC(8, 3)
AS
BEGIN
    INSERT INTO [Inventory].[food_nutrients] (food_id, nutrient_id, amount_per_serving)
    VALUES (@FoodId, @NutrientId, @AmountPerServing);
END