CREATE PROCEDURE [Wine].[usp_Bottle_GetAllByCountryId]
    @CountryId INT
AS
BEGIN
    SELECT * FROM [Wine].[Bottle] WHERE CountryID = @CountryId ORDER BY BottleNumber;
END