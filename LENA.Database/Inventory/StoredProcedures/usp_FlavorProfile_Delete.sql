CREATE PROCEDURE [Inventory].[usp_FlavorProfile_Delete]
    @FlavorId INT
AS
BEGIN
    DELETE FROM [Inventory].[flavor_profiles] WHERE flavor_id = @FlavorId;
END