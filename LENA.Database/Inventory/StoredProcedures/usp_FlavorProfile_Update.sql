CREATE PROCEDURE [Inventory].[usp_FlavorProfile_Update]
    @FlavorId INT,
    @FlavorName VARCHAR(50),
    @IsActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [Inventory].[flavor_profiles]
    SET flavor_name = @FlavorName,
        is_active = @IsActive
    WHERE flavor_id = @FlavorId;

    SELECT @@ROWCOUNT;
END
