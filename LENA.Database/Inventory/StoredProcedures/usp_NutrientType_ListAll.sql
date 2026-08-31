CREATE PROCEDURE [Inventory].[usp_NutrientType_ListAll]
AS
BEGIN
    SELECT nutrient_id AS NutrientId,
           nutrient_name AS NutrientName,
           unit_of_measure AS UnitOfMeasure
    FROM [Inventory].[nutrient_types]
    ORDER BY nutrient_name;
END