CREATE PROCEDURE [Inventory].[usp_Item_Delete]
    @ItemID INT,
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [Inventory].[UserItem] WHERE [UserID] = @UserID AND [ItemID] = @ItemID;

    SELECT @@ROWCOUNT;
END
