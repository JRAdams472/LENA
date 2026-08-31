CREATE PROCEDURE [Inventory].[usp_NutrientType_Update]
    @NutrientId INT,
    @NutrientName VARCHAR(100),
    @UnitOfMeasure VARCHAR(20)
AS
BEGIN
    UPDATE [Inventory].[nutrient_types]
    SET nutrient_name = @NutrientName,
        unit_of_measure = @UnitOfMeasure
    WHERE nutrient_id = @NutrientId;
END