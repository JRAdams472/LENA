CREATE PROCEDURE [Wine].[usp_Vintage_GetAllActive]
AS
BEGIN
    SELECT * FROM [Wine].[Vintage] WHERE IsActive = 1 ORDER BY Year;
END