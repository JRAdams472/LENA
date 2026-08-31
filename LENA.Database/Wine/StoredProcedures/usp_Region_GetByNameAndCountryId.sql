CREATE PROCEDURE [Wine].[usp_Region_GetByNameAndCountryId]
    @Name NVARCHAR(100),
    @CountryId INT
AS
BEGIN
    SELECT * FROM [Wine].[Region] WHERE RegionName = @Name AND CountryID = @CountryId;
END