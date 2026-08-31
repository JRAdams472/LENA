CREATE PROCEDURE [Inventory].[usp_FlavorProfile_Create]
    @FlavorName VARCHAR(50),
    @IsActive BIT = 1
AS
BEGIN
    INSERT INTO [Inventory].[flavor_profiles] (flavor_name, is_active)
    VALUES (@FlavorName, @IsActive);
    SELECT CAST(SCOPE_IDENTITY() as int);
END