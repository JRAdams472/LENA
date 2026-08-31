CREATE PROCEDURE [Wine].[usp_Region_GetAllByCountryId]
    @CountryId INT
AS
BEGIN
    SELECT * FROM [Wine].[Region] WHERE CountryID = @CountryId ORDER BY RegionName;
END