CREATE PROCEDURE [Inventory].[usp_FlavorProfile_ListAll]
AS
BEGIN
    SELECT flavor_id AS FlavorId,
           flavor_name AS FlavorName,
           is_active AS IsActive
    FROM [Inventory].[flavor_profiles]
    ORDER BY flavor_name;
END