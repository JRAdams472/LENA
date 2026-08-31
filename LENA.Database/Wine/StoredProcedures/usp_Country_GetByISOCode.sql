CREATE PROCEDURE [Wine].[usp_Country_GetByISOCode]
    @ISOCode NVARCHAR(10)
AS
BEGIN
    SELECT * FROM [Wine].[Country] WHERE ISOCode = @ISOCode;
END