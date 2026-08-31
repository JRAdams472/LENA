CREATE PROCEDURE [Wine].[usp_Bottle_GetById]
    @Id INT
AS
BEGIN
    SELECT * FROM [Wine].[Bottle] WHERE BottleID = @Id;
END