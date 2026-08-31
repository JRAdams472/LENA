CREATE PROCEDURE [Inventory].[usp_NutrientType_GetById]
    @Id INT
AS
BEGIN
    SELECT nutrient_id AS NutrientId,
           nutrient_name AS NutrientName,
           unit_of_measure AS UnitOfMeasure
    FROM [Inventory].[nutrient_types]
    WHERE nutrient_id = @Id;
END