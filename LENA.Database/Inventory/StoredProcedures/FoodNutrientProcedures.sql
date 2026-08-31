CREATE PROCEDURE [Inventory].[usp_FoodNutrient_ListAll]
AS
BEGIN
    SELECT food_id AS FoodId,
           nutrient_id AS NutrientId,
           amount_per_serving AS AmountPerServing
    FROM [Inventory].[food_nutrients];
END
GO

CREATE PROCEDURE [Inventory].[usp_FoodNutrient_GetById]
    @Id INT
AS
BEGIN
    SELECT food_id AS FoodId,
           nutrient_id AS NutrientId,
           amount_per_serving AS AmountPerServing
    FROM [Inventory].[food_nutrients]
    WHERE food_id = @Id;
END
GO

CREATE PROCEDURE [Inventory].[usp_FoodNutrient_GetByName]
    @Name VARCHAR(100)
AS
BEGIN
    RETURN;
END
GO

CREATE PROCEDURE [Inventory].[usp_FoodNutrient_Create]
    @FoodId INT,
    @NutrientId INT,
    @AmountPerServing NUMERIC(8, 3)
AS
BEGIN
    INSERT INTO [Inventory].[food_nutrients] (food_id, nutrient_id, amount_per_serving)
    VALUES (@FoodId, @NutrientId, @AmountPerServing);
END
GO

CREATE PROCEDURE [Inventory].[usp_FoodNutrient_Update]
    @FoodId INT,
    @NutrientId INT,
    @AmountPerServing NUMERIC(8, 3)
AS
BEGIN
    UPDATE [Inventory].[food_nutrients]
    SET amount_per_serving = @AmountPerServing
    WHERE food_id = @FoodId AND nutrient_id = @NutrientId;
END
GO

CREATE PROCEDURE [Inventory].[usp_FoodNutrient_Delete]
    @FoodId INT,
    @NutrientId INT
AS
BEGIN
    DELETE FROM [Inventory].[food_nutrients] WHERE food_id = @FoodId AND nutrient_id = @NutrientId;
END
GO

CREATE PROCEDURE [Inventory].[usp_FoodNutrient_GetByFoodId]
    @FoodId INT
AS
BEGIN
    SELECT food_id AS FoodId,
           nutrient_id AS NutrientId,
           amount_per_serving AS AmountPerServing
    FROM [Inventory].[food_nutrients]
    WHERE food_id = @FoodId;
END
GO

CREATE PROCEDURE [Inventory].[usp_FoodNutrient_GetByNutrientId]
    @NutrientId INT
AS
BEGIN
    SELECT food_id AS FoodId,
           nutrient_id AS NutrientId,
           amount_per_serving AS AmountPerServing
    FROM [Inventory].[food_nutrients]
    WHERE nutrient_id = @NutrientId;
END
GO

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
GO
