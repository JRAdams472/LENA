CREATE PROCEDURE [Inventory].[usp_FoodFlavor_Create]
    @FoodId INT,
    @FlavorId INT,
    @IntensityScore INT
AS
BEGIN
    INSERT INTO [Inventory].[food_flavors] (food_id, flavor_id, intensity_score)
    VALUES (@FoodId, @FlavorId, @IntensityScore);
END