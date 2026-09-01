CREATE PROCEDURE [Inventory].[usp_FlavorProfile_ListAll]
    @PageNumber INT = 1,
    @PageSize INT = 25
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    SELECT flavor_id AS FlavorId,
           flavor_name AS FlavorName,
           is_active AS IsActive
    FROM [Inventory].[flavor_profiles]
    ORDER BY flavor_name
        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;

    SELECT COUNT(*) FROM [Inventory].[flavor_profiles];
END