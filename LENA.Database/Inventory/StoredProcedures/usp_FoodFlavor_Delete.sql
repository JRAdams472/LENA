CREATE PROCEDURE [Inventory].[usp_FoodFlavor_Delete]
    @FoodId INT,
    @FlavorId INT
AS
BEGIN
    DELETE FROM [Inventory].[food_flavors] WHERE food_id = @FoodId AND flavor_id = @FlavorId;
END