CREATE PROCEDURE [Wine].[usp_Country_GetByName]
    @Name NVARCHAR(100)
AS
BEGIN
    SELECT * FROM [Wine].[Country] WHERE CountryName = @Name;
END