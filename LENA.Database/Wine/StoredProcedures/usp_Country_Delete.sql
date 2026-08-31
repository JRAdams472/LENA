CREATE PROCEDURE [Wine].[usp_Country_Delete]
    @CountryID INT
AS
BEGIN
    DELETE FROM [Wine].[Country] WHERE CountryID = @CountryID;
END