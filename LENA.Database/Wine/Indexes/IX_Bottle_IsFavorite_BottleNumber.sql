CREATE NONCLUSTERED INDEX [IX_Bottle_IsFavorite_BottleNumber]
    ON [Wine].[Bottle] ([IsFavorite] ASC, [BottleNumber] ASC);