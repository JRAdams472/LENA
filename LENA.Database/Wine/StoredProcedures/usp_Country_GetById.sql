CREATE PROCEDURE [Wine].[usp_Country_GetById]
    @Id INT
AS
BEGIN
    SELECT * FROM [Wine].[Country] WHERE CountryID = @Id;
END