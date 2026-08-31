CREATE PROCEDURE [Wine].[usp_Country_ListAll]
AS
BEGIN
    SELECT * FROM [Wine].[Country] ORDER BY CountryName;
END