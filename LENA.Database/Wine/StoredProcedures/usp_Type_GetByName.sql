CREATE PROCEDURE [Wine].[usp_Type_GetByName]
    @Name NVARCHAR(100)
AS
BEGIN
    SELECT * FROM [Wine].[Type] WHERE TypeName = @Name;
END