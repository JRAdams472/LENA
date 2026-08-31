CREATE PROCEDURE [Inventory].[usp_NutrientType_Delete]
    @NutrientId INT
AS
BEGIN
    DELETE FROM [Inventory].[nutrient_types] WHERE nutrient_id = @NutrientId;
END