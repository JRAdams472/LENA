CREATE NONCLUSTERED INDEX [IX_Bottle_RegionID_BottleNumber]
    ON [Wine].[Bottle] ([RegionID] ASC, [BottleNumber] ASC);