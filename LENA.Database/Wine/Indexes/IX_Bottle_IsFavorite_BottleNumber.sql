CREATE NONCLUSTERED INDEX [IX_UserBottle_UserID_IsFavorite_BottleNumber]
    ON [Wine].[UserBottle] ([UserID] ASC, [IsFavorite] ASC, [BottleNumber] ASC);