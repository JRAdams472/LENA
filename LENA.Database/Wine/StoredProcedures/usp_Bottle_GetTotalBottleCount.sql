CREATE PROCEDURE [Wine].[usp_Bottle_GetTotalBottleCount]
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*)
    FROM [Wine].[UserBottle]
    WHERE UserID = @UserID;
END