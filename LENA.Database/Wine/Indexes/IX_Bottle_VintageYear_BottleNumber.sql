CREATE NONCLUSTERED INDEX [IX_Bottle_VintageYear_BottleNumber]
    ON [Wine].[Bottle] ([VintageYear] ASC, [BottleNumber] ASC);