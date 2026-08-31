CREATE PROCEDURE [Inventory].[usp_Item_SetFavorite]
    @ItemID INT,
    @IsFavorite BIT
AS
BEGIN
    UPDATE [Inventory].[Item] SET [IsFavorite] = @IsFavorite WHERE [ItemID] = @ItemID;
END