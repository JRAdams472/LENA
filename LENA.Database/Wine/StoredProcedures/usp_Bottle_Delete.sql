CREATE PROCEDURE [Wine].[usp_Bottle_Delete]
    @UserID INT,
    @BottleID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [Wine].[UserBottle]
    WHERE UserID = @UserID AND BottleID = @BottleID;

    SELECT @@ROWCOUNT;
END
