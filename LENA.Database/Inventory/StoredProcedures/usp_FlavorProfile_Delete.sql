CREATE PROCEDURE [Inventory].[usp_FlavorProfile_Delete]
    @FlavorId INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [Inventory].[flavor_profiles] WHERE flavor_id = @FlavorId;

    SELECT @@ROWCOUNT;
END
