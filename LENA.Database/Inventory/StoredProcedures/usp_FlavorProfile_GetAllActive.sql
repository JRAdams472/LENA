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