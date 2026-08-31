CREATE NONCLUSTERED INDEX [IX_Bottle_TypeID_BottleNumber]
    ON [Wine].[Bottle] ([TypeID] ASC, [BottleNumber] ASC);