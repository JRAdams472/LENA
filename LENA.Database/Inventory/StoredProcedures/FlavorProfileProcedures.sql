CREATE PROCEDURE [Inventory].[usp_FlavorProfile_ListAll]
AS
BEGIN
    SELECT flavor_id AS FlavorId,
           flavor_name AS FlavorName,
           is_active AS IsActive
    FROM [Inventory].[flavor_profiles]
    ORDER BY flavor_name;
END
GO

CREATE PROCEDURE [Inventory].[usp_FlavorProfile_GetById]
    @Id INT
AS
BEGIN
    SELECT flavor_id AS FlavorId,
           flavor_name AS FlavorName,
           is_active AS IsActive
    FROM [Inventory].[flavor_profiles]
    WHERE flavor_id = @Id;
END
GO

CREATE PROCEDURE [Inventory].[usp_FlavorProfile_GetByName]
    @Name VARCHAR(50)
AS
BEGIN
    SELECT flavor_id AS FlavorId,
           flavor_name AS FlavorName,
           is_active AS IsActive
    FROM [Inventory].[flavor_profiles]
    WHERE flavor_name = @Name;
END
GO

CREATE PROCEDURE [Inventory].[usp_FlavorProfile_Create]
    @FlavorName VARCHAR(50),
    @IsActive BIT = 1
AS
BEGIN
    INSERT INTO [Inventory].[flavor_profiles] (flavor_name, is_active)
    VALUES (@FlavorName, @IsActive);
    SELECT CAST(SCOPE_IDENTITY() as int);
END
GO

CREATE PROCEDURE [Inventory].[usp_FlavorProfile_Update]
    @FlavorId INT,
    @FlavorName VARCHAR(50),
    @IsActive BIT = 1
AS
BEGIN
    UPDATE [Inventory].[flavor_profiles]
    SET flavor_name = @FlavorName,
        is_active = @IsActive
    WHERE flavor_id = @FlavorId;
END
GO

CREATE PROCEDURE [Inventory].[usp_FlavorProfile_Delete]
    @FlavorId INT
AS
BEGIN
    DELETE FROM [Inventory].[flavor_profiles] WHERE flavor_id = @FlavorId;
END
GO

CREATE PROCEDURE [Inventory].[usp_FlavorProfile_GetAllActive]
AS
BEGIN
    SELECT flavor_id AS FlavorId,
           flavor_name AS FlavorName,
           is_active AS IsActive
    FROM [Inventory].[flavor_profiles]
    WHERE is_active = 1
    ORDER BY flavor_name;
END
GO
