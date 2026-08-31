CREATE PROCEDURE [Inventory].[usp_NutrientType_Create]
    @NutrientName VARCHAR(100),
    @UnitOfMeasure VARCHAR(20)
AS
BEGIN
    INSERT INTO [Inventory].[nutrient_types] (nutrient_name, unit_of_measure)
    VALUES (@NutrientName, @UnitOfMeasure);
    SELECT CAST(SCOPE_IDENTITY() as int);
END