CREATE PROCEDURE [Wine].[usp_Bottle_Delete]
    @BottleID INT
AS
BEGIN
    DELETE FROM [Wine].[Bottle] WHERE BottleID = @BottleID;
END