CREATE PROCEDURE [Inventory].[usp_Item_Delete]
    @ItemID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [Inventory].[Item] WHERE [ItemID] = @ItemID;

    SELECT @@ROWCOUNT;
END
