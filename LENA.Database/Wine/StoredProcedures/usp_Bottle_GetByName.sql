CREATE PROCEDURE [Wine].[usp_Bottle_GetByName]
    @Name NVARCHAR(200)
AS
BEGIN
    SELECT * FROM [Wine].[Bottle] WHERE Vineyard = @Name;
END