CREATE PROCEDURE [Wine].[usp_Country_Delete]
    @CountryID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [Wine].[Country] WHERE CountryID = @CountryID;

    SELECT @@ROWCOUNT;
END
