CREATE PROCEDURE [Inventory].[usp_FoodFlavor_Update]
    @FoodId INT,
    @FlavorId INT,
    @IntensityScore INT
AS
BEGIN
    UPDATE [Inventory].[food_flavors]
    SET intensity_score = @IntensityScore
    WHERE food_id = @FoodId AND flavor_id = @FlavorId;
END