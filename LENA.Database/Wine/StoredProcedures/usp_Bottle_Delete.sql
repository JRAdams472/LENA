CREATE PROCEDURE [Wine].[usp_Bottle_Delete]
    @BottleID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [Wine].[Bottle] WHERE BottleID = @BottleID;

    SELECT @@ROWCOUNT;
END
