CREATE PROCEDURE [Wine].[usp_Country_GetAllActive]
AS
BEGIN
    SELECT * FROM [Wine].[Country] WHERE IsActive = 1 ORDER BY CountryName;
END