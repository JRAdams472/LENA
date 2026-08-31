CREATE PROCEDURE [Inventory].[usp_FoodNutrient_Update]
    @FoodId INT,
    @NutrientId INT,
    @AmountPerServing NUMERIC(8, 3)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [Inventory].[food_nutrients]
    SET amount_per_serving = @AmountPerServing
    WHERE food_id = @FoodId AND nutrient_id = @NutrientId;

    SELECT @@ROWCOUNT;
END
