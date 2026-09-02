CREATE PROCEDURE [Wine].[usp_Bottle_SetFavorite]
    @UserID INT,
    @BottleID INT,
    @IsFavorite BIT,
    @CreatedBy NVARCHAR(100),
    @CreateDate DATETIME2,
    @LastUpdatedBy NVARCHAR(100),
    @LastUpdatedDate DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    IF @IsFavorite = 1
    BEGIN
        MERGE [Wine].[UserBottle] AS target
        USING (VALUES (@UserID, @BottleID, @IsFavorite)) AS source (UserID, BottleID, IsFavorite)
        ON target.[UserID] = source.UserID AND target.[BottleID] = source.BottleID
        WHEN MATCHED THEN
            UPDATE SET [IsFavorite] = 1, [LastUpdatedBy] = @LastUpdatedBy, [LastUpdatedDate] = @LastUpdatedDate
        WHEN NOT MATCHED THEN
            INSERT ([UserID], [BottleID], [BottleNumber], [BottleSize], [Quantity], [PurchaseDate], [PurchasePrice], [StorageTemp], [Location], [Notes], [IsFavorite], [CreatedBy], [CreateDate])
            VALUES (source.UserID, source.BottleID, NULL, '750ml', 0, NULL, NULL, NULL, NULL, NULL, 1, @CreatedBy, @CreateDate);
    END
    ELSE
    BEGIN
        UPDATE [Wine].[UserBottle]
        SET [IsFavorite] = 0, [LastUpdatedBy] = @LastUpdatedBy, [LastUpdatedDate] = @LastUpdatedDate
        WHERE [UserID] = @UserID AND [BottleID] = @BottleID;
    END
END
