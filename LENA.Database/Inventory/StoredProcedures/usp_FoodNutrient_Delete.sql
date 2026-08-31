CREATE PROCEDURE [Inventory].[usp_FoodNutrient_Delete]
    @FoodId INT,
    @NutrientId INT
AS
BEGIN
    DELETE FROM [Inventory].[food_nutrients] WHERE food_id = @FoodId AND nutrient_id = @NutrientId;
END