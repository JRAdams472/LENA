CREATE NONCLUSTERED INDEX [IX_Bottle_CountryID_BottleNumber]
    ON [Wine].[Bottle] ([CountryID] ASC, [BottleNumber] ASC);