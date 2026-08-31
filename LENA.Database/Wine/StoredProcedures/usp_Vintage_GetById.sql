CREATE PROCEDURE [Wine].[usp_Vintage_GetById]
    @Id INT
AS
BEGIN
    SELECT * FROM [Wine].[Vintage] WHERE VintageID = @Id;
END