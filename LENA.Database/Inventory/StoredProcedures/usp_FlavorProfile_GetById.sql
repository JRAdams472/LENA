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