CREATE PROCEDURE [Inventory].[usp_FoodFlavor_GetById]
    @Id INT
AS
BEGIN
    SELECT food_id AS FoodId,
           flavor_id AS FlavorId,
           intensity_score AS IntensityScore
    FROM [Inventory].[food_flavors]
    WHERE food_id = @Id;
END