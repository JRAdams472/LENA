CREATE PROCEDURE [Wine].[usp_Vintage_GetByYear]
    @Year INT
AS
BEGIN
    SELECT * FROM [Wine].[Vintage] WHERE Year = @Year;
END