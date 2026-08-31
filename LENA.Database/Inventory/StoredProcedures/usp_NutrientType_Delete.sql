CREATE PROCEDURE [Inventory].[usp_NutrientType_Delete]
    @NutrientId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [Inventory].[nutrient_types] WHERE nutrient_id = @NutrientId;

    SELECT @@ROWCOUNT;
END
